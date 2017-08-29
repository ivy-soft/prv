namespace TheGarage.Services.AutomaticPayment.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Logic.Contracts;

    public class InvoiceAmountCalculator : IInvoiceAmountCalculator
    {

        public decimal TotalAmount(IEnumerable<Card> cards, /*, IEnumerable<ChargeCode> chargeCodes, */decimal garageTax)
        {
            decimal monthlyDueCharge = 0;
            foreach (var card in cards)
            {
                //var chargeCode = /*this.chargeCodeProvider.GetById(card.ChargeCodeId);*/*/card.ChargeCode.chargeCodes.ToList().SingleOrDefault(c => c.Id == card.ChargeCodeId);*/
                monthlyDueCharge += card.ChargeCode.Amount + (card.ChargeCode.Amount * (garageTax / 100));
            }

            return monthlyDueCharge;
        }
    }
}
