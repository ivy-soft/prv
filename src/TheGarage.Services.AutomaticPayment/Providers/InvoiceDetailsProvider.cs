namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Factories.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class InvoiceDetailsProvider : IInvoiceDetailsProvider
    {
        public IEnumerable<InvoiceDetail> Create(IEnumerable<Card> cards, Garage garage, Invoice invoice)
        {
            decimal chargeCodeAmountSum = 0;
            var cardCollection = cards.ToList();

            foreach (var card in cardCollection)
            {
                if (card.ChargeCode == null)
                {
                    throw new ArgumentNullException("Charge code is not included!");
                }

                chargeCodeAmountSum += card.ChargeCode.Amount;
            }

            var invoiceDetailsCollection = new List<InvoiceDetail>();
            foreach (var card in cardCollection)
            {
                var invoiceDetails = new InvoiceDetail();
                invoiceDetails.TAX = garage.TAX;
                invoiceDetails.ChargeCode = card.ChargeCode.Name;
                invoiceDetails.Amount = card.ChargeCode.Amount;
                invoiceDetails.Cards = card.CardNumber.ToString();
                invoiceDetails.InvoiceId = invoice.Id;
                invoiceDetails.StartDatePaid = invoice.StartDate;
                invoiceDetails.EndDatePaid = invoice.EndDate;
                invoiceDetails.GarageName = garage.Name;
                int paidDays = (invoice.EndDate - invoice.StartDate).Days + 1;
                invoiceDetails.PaidDays = paidDays;
                var dailyRateSum = (invoice.Amount / ((garage.TAX + 100) / 100)) / paidDays;
                invoiceDetails.DailyRate = (card.ChargeCode.Amount / chargeCodeAmountSum) * dailyRateSum;

                invoiceDetailsCollection.Add(invoiceDetails);
            }

            return invoiceDetailsCollection;
        }
    }
}
