namespace Tutor.Application.Common.Interfaces
{
    public interface IEmailService
    {
        string GetEmailTemplate(string templateName);
        string ReplacePlaceholders(string template, Dictionary<string, string> placeholders);
        Task SendEmailAsync(string recipientEmail, string subject, string templateName, Dictionary<string, string> placeholders);
    }
}
