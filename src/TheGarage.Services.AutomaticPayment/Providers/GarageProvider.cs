namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using TheGarage.Data;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class GarageProvider : IGarageProvider
    {
        public Garage UpdateMonthlyInvoiceData(Garage garage)
        {
            garage.SequenceInvoiceNumber++;

            return garage;
        }
    }
}
