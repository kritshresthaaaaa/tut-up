using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tutor.Application.Common.Interfaces;
using Tutor.Infrastructure.Configuration.EmailConfiguration;

namespace Tutor.Infrastructure.Data
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public string GetEmailTemplate(string templateName)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", templateName);
            return File.ReadAllText(templatePath);
        }

        public string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }
            return template;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string templateName, Dictionary<string, string> placeholders)
        {
            var template = GetEmailTemplate(templateName);
            var emailBody = ReplacePlaceholders(template, placeholders);

            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                /*                EnableSsl = _emailSettings.EnableSsl
                */
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = emailBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(recipientEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

}
