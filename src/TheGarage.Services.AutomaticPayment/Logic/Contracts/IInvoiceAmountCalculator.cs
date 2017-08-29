namespace TheGarage.Services.AutomaticPayment.Logic.Contracts
{
    using System.Collections.Generic;
    using TheGarage.Data.Models;

    public interface IInvoiceAmountCalculator
    {

        decimal TotalAmount(IEnumerable<Card> cards, /*IEnumerable<ChargeCode> chargeCodes, */decimal garageTax);
    }
}
