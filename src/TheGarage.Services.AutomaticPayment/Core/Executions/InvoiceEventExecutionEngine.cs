namespace TheGarage.Services.AutomaticPayment.Core.Executions
{
    using Bytes2you.Validation;
    using System;
    using System.Linq;
    using TheGarage.Data;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Core.Executions.Contracts;
    using TheGarage.Services.AutomaticPayment.Facade;
    using TheGarage.Services.AutomaticPayment.Facade.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class InvoiceEventExecutionEngine : IInvoiceEventExecutionEngine
    {
        private readonly IInvoiceEventFacade invoiceEventFacade;
        private readonly ICardProvider cardProvider;
        private readonly IDataProvider dataProvider;
        private readonly ITheGarageData data;
        private readonly IEmailMessageFacade emailMessageFacade;

        public InvoiceEventExecutionEngine(ITheGarageData data, IInvoiceEventFacade invoiceEventFacade, IDataProvider dataProvider, ICardProvider cardProvider, IEmailMessageFacade emailMessageFacade)
        {
            this.data = data;
            this.invoiceEventFacade = invoiceEventFacade;
            this.dataProvider = dataProvider;
            this.cardProvider = cardProvider;
            this.emailMessageFacade = emailMessageFacade;
        }

        public void Execute(Garage garage)
        {
            Guard.WhenArgument(garage, "garage").IsNull().Throw();
            Guard.WhenArgument(garage.Company, "company").IsNull().Throw();

            var invoiceUsers = this.dataProvider.InvoiceUsers(garage);

            foreach (var user in invoiceUsers)
            {
                if (this.Validation(user))
                {
                    var filteredCards = this.cardProvider.FilteringValidCardsForInvoice(user.Cards).ToList();

                    var invoice = this.invoiceEventFacade.CreateInvoice(filteredCards, garage, garage.Company, user);
                    this.data.Invoices.Add(invoice);

                    var invoiceDetails = this.invoiceEventFacade.CreateInvoiceDetails(filteredCards, garage, invoice);
                    foreach (var entity in invoiceDetails)
                    {
                        this.data.InvoiceDetails.Add(entity);
                    }

                    var garageEntity = this.invoiceEventFacade.UpdateMonthlyInvoiceData(garage);
                    this.data.Garages.Update(garageEntity);

                    var userEntity = this.invoiceEventFacade.UpdateMonthlyInvoiceData(user, invoice.Amount);
                    this.data.Users.Update(userEntity);

                    // Send message and return message entity;
                    if (!user.AutoCharge)
                    {
                        var emailMessage = this.emailMessageFacade.Send(user, garage, invoice);
                        this.data.EmailMessages.Add(emailMessage);
                    }
                }
            }

            //this.data.SaveChanges();
        }

        private bool Validation(User user)
        {
            bool isEventDayForSubscriber = false;

            if (user.PaidTrough != null)
            {
                var paidThrough = (DateTime)user.PaidTrough;
                isEventDayForSubscriber = paidThrough.Year == DateTimeProvider.Current.Now.Year && paidThrough.Month == DateTimeProvider.Current.Now.Month;
            }

            var isValid = isEventDayForSubscriber == true || user.HasProrateInvoiceUnpaid;

            return isValid;
        }
    }
}
