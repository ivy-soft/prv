namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System;
    using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Contracts;
    using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Factories.Contracts;
    using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Models;

    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        private readonly IEmailMessageTemplateFactory emailMessageTemplateFactory;

        public EmailTemplateProvider(IEmailMessageTemplateFactory emailMessageTemplateFactory)
        {
            this.emailMessageTemplateFactory = emailMessageTemplateFactory;
        }

        public EmailMessageTemplateModel CreateEmailTemplate(string templateSubject, string templateBody, string subscriber, string garageName, string invoiceNumber, DateTime invoiceStartDate)
        {
                var body = templateBody;
                var subject = templateSubject;
                body = body.Replace("{{garageName}}", garageName);
                body = body.Replace("{{customer}}", subscriber);/*user.IsCompany ? user.CustomerCompany.Name : user.Customer.FirstName + " " + user.Customer.LastName);*/

                body = body.Replace("{{invoiceStartDate}}", invoiceStartDate.ToString("MM/dd/yyyy"));
                subject = subject.Replace("{{garageName}}", garageName);
                subject = subject.Replace("{{invoiceNumber}}", invoiceNumber);

            var emailMassageModel = this.emailMessageTemplateFactory.CreateEmailMessageTemplateModel(subject, body);

            return emailMassageModel;
        }
    }
}
