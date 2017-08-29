namespace TheGarage.EmailService.Contracts
{
    using System.Net.Mail;

    public interface ISmtpClientService
    {
        void Send(MailMessage message);
    }
}
