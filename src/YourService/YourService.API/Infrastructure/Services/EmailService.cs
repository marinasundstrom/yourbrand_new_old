﻿using System.Net.Mail;

namespace YourBrand.YourService.API.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpClient _smtpClient;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;

        // Fake server
        _smtpClient = new SmtpClient("localhost", 25);
    }

    public async Task SendEmail(string recipient, string subject, string body)
    {
        var message = new MailMessage(new MailAddress("noreply@orderapp-test.com", "Order app"), new MailAddress(recipient))
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
            BodyEncoding = System.Text.Encoding.UTF8
        };

        await _smtpClient.SendMailAsync(message);

        _logger.LogInformation("Email was sent.");
    }
}