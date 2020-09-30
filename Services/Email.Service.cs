using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Vote.Services
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Почти платформа Голос", "aspvote@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(email, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new BodyBuilder()
            {
                HtmlBody = $"<div style=\"color: purple;\">{message}</div>",
            }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 25, false);

            await client.AuthenticateAsync("aspvote@gmail.com", "147896325xXx");

            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
