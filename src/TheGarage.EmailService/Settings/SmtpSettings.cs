namespace TheGarage.EmailService.Settings
{
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Web.WebPages;
    using TheGarage.EmailService.Settings.Contracts;

    public class SmtpSettings : ISmtpSettings
    {
        public string SmtpServer
        {
            get
            {
                return ConfigurationManager.AppSettings["smtp.server"];
            }
        }

        public int SmtpPort
        {
            get
            {
                return ConfigurationManager.AppSettings["smtp.port"].AsInt();
            }
        }

        public string SmtpUser
        {
            get
            {
                return ConfigurationManager.AppSettings["smtp.user"];
            }
        }

        public string SmtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["smtp.password"];
            }
        }

        public SmtpDeliveryMethod NetworkSmtpDeliveryMethod
        {
            get
            {
                return SmtpDeliveryMethod.Network;
            }
        }

        public ICredentialsByHost CreateNetworkCredential(string userName, string password)
        {
            return new NetworkCredential(userName, password);
        }

        public SmtpClient CreateSmtpClient(string host, int port, bool enableSSL, SmtpDeliveryMethod smtpDeliveryMethod, bool useDefaultCredentials, ICredentialsByHost credentialsByHost)
        {
            var smtp = new SmtpClient();
            smtp.Host = host;
            smtp.Port = port;
            smtp.EnableSsl = enableSSL;
            smtp.DeliveryMethod = smtpDeliveryMethod;
            smtp.UseDefaultCredentials = useDefaultCredentials;
            smtp.Credentials = credentialsByHost;

            return smtp;
        }
    }
}
