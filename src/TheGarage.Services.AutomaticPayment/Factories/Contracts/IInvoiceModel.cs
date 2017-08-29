namespace TheGarage.Services.AutomaticPayment.Factories.Contracts
{
    using System;
    using TheGarage.Data.Models;

    public interface IInvoiceModel
    {
        Invoice CreateInvoice(Guid garageId, string garageName, string userId, string clientName, string number, decimal amount);
    }
}
