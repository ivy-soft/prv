namespace TheGarage.Services.AutomaticPayment.Providers.Contracts
{
    using System.Collections.Generic;
    using TheGarage.Data.Models;

    public interface ICardProvider
    {
        IEnumerable<Card> FilteringValidCardsForInvoice(IEnumerable<Card> cards);
    }
}