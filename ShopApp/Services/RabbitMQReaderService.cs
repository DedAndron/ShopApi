using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.Configuration;
using System.Text;
using System.Text.Json;
namespace Shop.Api.Services;

/// <summary>
/// Робота класа запускається у фоновому режимі, всі логі пишутся в 1 консоль
/// </summary>
public class RabbitMQReaderService : BackgroundService
{
    private const string OrdersQueue = "Orders";
    private readonly ILogger<RabbitMQReaderService> _logger;
    private readonly RabbitMQSettings _rabbitMqSettings;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public RabbitMQReaderService(
        ILogger<RabbitMQReaderService> logger,
        IOptions<RabbitMQSettings> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _rabbitMqSettings = options.Value;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(OrdersQueue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);
        await _channel.BasicQosAsync(0, 1, false, stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, e) =>
        {
            var body = e.Body.ToArray();

            var json = Encoding.UTF8.GetString(body);

            QueuedOrderDTO? message;
            try
            {
                message = JsonSerializer.Deserialize<QueuedOrderDTO>(json);
            }
            catch (JsonException exception)
            {
                _logger.LogWarning(exception, "Discarded malformed message from the Orders queue");
                await _channel.BasicAckAsync(e.DeliveryTag, false, stoppingToken);
                return;
            }

            if (message == null)
            {
                await _channel.BasicAckAsync(e.DeliveryTag, false, stoppingToken);
                return;
            }

            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var processor = scope.ServiceProvider.GetRequiredService<IOrderQueueProcessor>();
                if (await processor.ProcessAsync(message, stoppingToken))
                    await _channel.BasicAckAsync(e.DeliveryTag, false, stoppingToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to process an order from the Orders queue");
                await _channel.BasicNackAsync(e.DeliveryTag, false, true, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: OrdersQueue,
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation(
            "RabbitMQ Reader started. Waiting messages...");

        // Замість Console.ReadLine()
        await Task.Delay(
            Timeout.Infinite,
            stoppingToken);
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "RabbitMQ Reader stopping...");

        if (_channel != null)
            await _channel.CloseAsync();

        if (_connection != null)
            await _connection.CloseAsync();

        await base.StopAsync(cancellationToken);
    }
}