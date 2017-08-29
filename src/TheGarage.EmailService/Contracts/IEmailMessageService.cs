namespace TheGarage.EmailService.Contracts
{
    using System.Collections.Generic;
    using TheGarage.Data.Models;

    public interface IEmailMessageService
    {
        ICollection<byte[]> PdfStreams { get; }

        EmailMessage Send(string userId, string toEmail, string fromEmail, string subject, string body, string sender);
    }
}
