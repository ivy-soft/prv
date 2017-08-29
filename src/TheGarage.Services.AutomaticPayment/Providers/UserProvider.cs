namespace TheGarage.Services.AutomaticPayment.Providers
{
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class UserProvider : IUserProvider
    {
        public User UpdateMonthlyInvoiceData(User user, decimal invoiceAmount)
        {
            user.Balance -= invoiceAmount;
            user.HasMonthlyInvoiceUnpaid = true;

            return user;
        }
    }
}
