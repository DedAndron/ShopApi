using Microsoft.Extensions.Options;
using Shop.Api.Interfaces;
using Shop.Infrastructure.Configuration;
using System.Net;
using System.Net.Mail;

namespace Shop.Infrastructure.Services;

public sealed class SmtpEmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendAsync(string recipient, string subject, string body, CancellationToken cancellationToken = default)
    {
        using var message = new MailMessage(_settings.From, recipient, subject, body)
        {
            IsBodyHtml = false
        };
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(_settings.Username))
            client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

        await client.SendMailAsync(message, cancellationToken);
    }
}
