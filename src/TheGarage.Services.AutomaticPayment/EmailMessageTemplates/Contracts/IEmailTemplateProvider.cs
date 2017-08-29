namespace TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Contracts
{
    using System;
    using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Models;

    public interface IEmailTemplateProvider
    {
        EmailMessageTemplateModel CreateEmailTemplate(string templateSubject, string templateBody, string subscriber, string garageName, string invoiceNumber, DateTime invoiceStartDate);
    }
}