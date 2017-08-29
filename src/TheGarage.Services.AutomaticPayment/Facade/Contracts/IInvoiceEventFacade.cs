namespace TheGarage.Services.AutomaticPayment.Facade.Contracts
{
    using System.Collections.Generic;
    using TheGarage.Data.Models;

    public interface IInvoiceEventFacade
    {
        Invoice CreateInvoice(IEnumerable<Card> filteredCards, Garage garage, Company company, User user);

        IEnumerable<InvoiceDetail> CreateInvoiceDetails(IEnumerable<Card> filteredCards, Garage garage, Invoice invoice);

        Garage UpdateMonthlyInvoiceData(Garage garage);

        User UpdateMonthlyInvoiceData(User user, decimal invoiceAmount);
    }
}
