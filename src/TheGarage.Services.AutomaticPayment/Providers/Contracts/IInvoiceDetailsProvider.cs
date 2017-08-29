namespace TheGarage.Services.AutomaticPayment.Providers.Contracts
{
    using System.Collections.Generic;
    using TheGarage.Data.Models;

    public interface IInvoiceDetailsProvider
    {
        IEnumerable<InvoiceDetail> Create(IEnumerable<Card> cards, Garage garage, Invoice invoice);
    }
}
