namespace TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Factories.Contracts
{
    using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Models;

    public interface IEmailMessageTemplateFactory
    {
        EmailMessageTemplateModel CreateEmailMessageTemplateModel(string subject, string body);
    }
}