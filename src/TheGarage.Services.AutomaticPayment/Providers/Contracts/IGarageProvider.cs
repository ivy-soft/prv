namespace TheGarage.Services.AutomaticPayment.Providers.Contracts
{
    using TheGarage.Data.Models;

    public interface IGarageProvider
    {
        Garage UpdateMonthlyInvoiceData(Garage garage);
    }
}