using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Vote.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var emailMessage = new MailMessage
            {
                From = (new MailAddress(_configuration["EmailSender:Email"], "Asp Vote")),
                Body = $"<div style=\"color: purple;\">{message}</div>",
                Subject = subject,
                IsBodyHtml = true
            };
            emailMessage.To.Add(new MailAddress(email, email));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential(_configuration["EmailSender:Email"], _configuration["EmailSender:Password"]);
            smtp.EnableSsl = true;
            smtp.Send(emailMessage);
        }
    }
}
