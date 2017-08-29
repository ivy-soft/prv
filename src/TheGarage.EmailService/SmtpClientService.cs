namespace TheGarage.EmailService
{
    using System.Net.Mail;
    using TheGarage.EmailService.Contracts;
    using TheGarage.EmailService.Settings.Contracts;

    public class SmtpClientService : ISmtpClientService
    {
        private readonly ISmtpSettings smtpSettings;

        public SmtpClientService(ISmtpSettings smtpSettings)
        {
            this.smtpSettings = smtpSettings;
        }

        public void Send(MailMessage message)
        {
            var credentialsByHost = this.smtpSettings.CreateNetworkCredential(this.smtpSettings.SmtpUser, this.smtpSettings.SmtpPassword);

            using (var client = this.smtpSettings.CreateSmtpClient(this.smtpSettings.SmtpServer, this.smtpSettings.SmtpPort, true, this.smtpSettings.NetworkSmtpDeliveryMethod, false, credentialsByHost))
            {
                client.Send(message);
            }
        }
    }
}
