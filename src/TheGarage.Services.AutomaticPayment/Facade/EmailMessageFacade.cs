using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGarage.Data.Models;
using TheGarage.EmailService.Contracts;
using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Contracts;
using TheGarage.Services.AutomaticPayment.Facade.Contracts;

namespace TheGarage.Services.AutomaticPayment.Facade
{
    public class EmailMessageFacade : IEmailMessageFacade
    {
        private readonly IEmailTemplateProvider emailTemplateProvider;
        private readonly IEmailMessageService emailMessageService;

        public EmailMessageFacade(IEmailTemplateProvider emailTemplateProvider, IEmailMessageService emailMessageService)
        {
            this.emailTemplateProvider = emailTemplateProvider;
            this.emailMessageService = emailMessageService;
        }

        public EmailMessage Send(User user, Garage garage, Invoice invoice)
        {
            var subscriber = string.Empty;
            if (user.IsCompany)
            {
                subscriber = user.CustomerCompany.Name;
            }
            else
            {
                subscriber = user.Customer.FirstName + " " + user.Customer.LastName;
            }

            var emailTemplateModel = this.emailTemplateProvider.CreateEmailTemplate(garage.TemplateSubjectInvoiceEventMail, garage.TemplateBodyMassageInvoiceEventMail, subscriber, garage.Name, invoice.Number, invoice.StartDate);

            // Send message and return message entity;
            var emailMessage = this.emailMessageService.Send(user.Id, user.Email, garage.InfoEmail, emailTemplateModel.Subject, emailTemplateModel.Body, "Automatic Payment System");

            return emailMessage;
        }
    }
}
