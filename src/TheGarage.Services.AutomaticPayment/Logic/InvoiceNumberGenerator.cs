namespace TheGarage.Services.AutomaticPayment.Logic
{
    using System;
    using TheGarage.Services.AutomaticPayment.Logic.Contracts;

    public class InvoiceNumberGenerator : IInvoiceNumberGenerator
    {
        public string Generate(int garageCompanySequenceNumber, int garageUniqueNumber, int garageSequenceInvoiceNumber)
        {
            var number = garageCompanySequenceNumber + 999 + garageUniqueNumber.ToString("000") + garageSequenceInvoiceNumber.ToString("0000000");

            return number;
        }
    }
}
