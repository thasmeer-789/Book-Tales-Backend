using BookTales.Application.Interfaces.Services;
using BookTales.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BookTales.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string body)
    {
        using var smtpClient = new SmtpClient(
            _settings.SmtpServer,
            _settings.Port);

        smtpClient.EnableSsl = true;

        smtpClient.Credentials = new NetworkCredential(
            _settings.Email,
            _settings.AppPassword);

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.Email),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}