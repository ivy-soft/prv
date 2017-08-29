namespace TheGarage.Services.AutomaticPayment.Facade
{
    using System.Collections.Generic;
    using System.Linq;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Facade.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class InvoiceEventFacade : IInvoiceEventFacade
    {
        private readonly IInvoiceProvider invoiceProvider;
        private readonly IInvoiceDetailsProvider invoiceDetailsProvider;
        private readonly IGarageProvider garageProvider;
        private readonly IUserProvider userProvider;

        public InvoiceEventFacade(IInvoiceProvider invoiceProvider, IInvoiceDetailsProvider invoiceDetailsProvider, IGarageProvider garageProvider, IUserProvider userProvider)
        {
            this.invoiceProvider = invoiceProvider;
            this.invoiceDetailsProvider = invoiceDetailsProvider;
            this.garageProvider = garageProvider;
            this.userProvider = userProvider;
        }

        public Invoice CreateInvoice(IEnumerable<Card> filteredCards, Garage garage, Company company, User user)
        {
           return this.invoiceProvider.Create(filteredCards, garage, company, user);
        }

        public IEnumerable<InvoiceDetail> CreateInvoiceDetails(IEnumerable<Card> filteredCards, Garage garage, Invoice invoice)
        {
            return this.invoiceDetailsProvider.Create(filteredCards, garage, invoice).ToList();
        }

        public Garage UpdateMonthlyInvoiceData(Garage garage)
        {
            return this.garageProvider.UpdateMonthlyInvoiceData(garage);
        }

        public User UpdateMonthlyInvoiceData(User user, decimal invoiceAmount)
        {
            return this.userProvider.UpdateMonthlyInvoiceData(user, invoiceAmount);
        }
    }
}
