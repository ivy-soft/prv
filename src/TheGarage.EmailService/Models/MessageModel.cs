namespace TheGarage.EmailService.Models
{
    using System.Net.Mail;
    using TheGarage.Data.Models;
    using TheGarage.EmailService.Models.Contracts;

    public class MessageModel : IMessageModel
    {
        public MailMessage CreateMailMessage(string from, string to, string subject, string body)
        {
            var message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            return message;
        }

        public EmailMessage CreateEmailMessage(string userId, string toEmail, string fromEmail, string subject, string body, string sender)
        {
            var model = new EmailMessage();
            model.UserId = userId;
            model.ToEmail = toEmail;
            model.FromEmail = fromEmail;
            model.Subject = subject;
            model.Body = body;
            model.Sender = sender;

            return model;
        }
    }
}
