namespace TheGarage.Services.AutomaticPayment.Factories.Contracts
{
    using System;
    using TheGarage.Data.Models;

    public interface IModelsFactory
    {
        //InvoiceDetail CreateMonthlyInvoiceDetails(string cardNumber, string chargeCodeName, decimal amount, decimal garageTax, int paidDays, decimal dailyRate, DateTime startDate, DateTime endDate, string garageName, Guid invoiceId);

        Invoice CreateMonthlyInvoiceBySystem(Guid garageId, string garageName, string userId, string subscriber, string number, decimal amount);
    }
}
