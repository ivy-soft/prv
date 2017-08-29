namespace TheGarage.EmailService.Models.Contracts
{
    using System.Net.Mail;
    using TheGarage.Data.Models;

    public interface IMessageModel
    {
        MailMessage CreateMailMessage(string from, string to, string subject, string body);

        EmailMessage CreateEmailMessage(string userId, string toEmail, string fromEmail, string subject, string body, string sender);
    }
}
