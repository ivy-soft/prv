namespace TheGarage.EmailService.Settings.Contracts
{
    using System.Net;
    using System.Net.Mail;

    public interface ISmtpSettings
    {
        string SmtpServer { get; }

        int SmtpPort { get; }

        string SmtpUser { get; }

        string SmtpPassword { get; }

        SmtpDeliveryMethod NetworkSmtpDeliveryMethod { get; }

        ICredentialsByHost CreateNetworkCredential(string userName, string password);

        SmtpClient CreateSmtpClient(string host, int port, bool enableSSL, SmtpDeliveryMethod smtpDeliveryMethod, bool useDefaultCredentials, ICredentialsByHost credentialsByHost);
    }
}
