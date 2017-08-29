namespace TheGarage.Services.AutomaticPayment.Providers.Contracts
{
    using TheGarage.Data.Models;

    public interface IUserProvider
    {
        User UpdateMonthlyInvoiceData(User user, decimal invoiceAmount);
    }
}