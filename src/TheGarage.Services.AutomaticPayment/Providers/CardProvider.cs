namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Bytes2you.Validation;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class CardProvider : ICardProvider
    {
        public IEnumerable<Card> FilteringValidCardsForInvoice(IEnumerable<Card> cards)
        {
            return cards
                .Where(card => card.IsActive
                    && !card.IsDeleted
                    && card.ValidFrom.Month <= DateTimeProvider.Current.Now.Month)
                .ToList();
        }
    }
}
