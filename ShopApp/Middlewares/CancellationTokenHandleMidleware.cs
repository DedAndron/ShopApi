namespace Shop.Api.Middlewares;

public class CancellationTokenHandleMidleware(RequestDelegate _next, ILogger<CancellationTokenHandleMidleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex) when(ex is OperationCanceledException)
        {
            _logger.LogError("Request canceled");
        }
    }
}
