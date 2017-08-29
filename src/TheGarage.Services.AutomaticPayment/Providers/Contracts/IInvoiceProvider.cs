namespace TheGarage.Services.AutomaticPayment.Providers.Contracts
{
    using System.Collections.Generic;
    using TheGarage.Data.Models;

    public interface IInvoiceProvider
    {
        Invoice Create(IEnumerable<Card> filteredCards, /*IEnumerable<ChargeCode> chargeCodes, */Garage garage, Company company, User user);
    }
}