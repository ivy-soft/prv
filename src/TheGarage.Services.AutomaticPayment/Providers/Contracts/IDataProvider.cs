namespace TheGarage.Services.AutomaticPayment.Providers.Contracts
{
    using System.Collections.Generic;
    using TheGarage.Data;
    using TheGarage.Data.Models;

    public interface IDataProvider
    {
        IEnumerable<Garage> GetAllGarages(/*ITheGarageData data*/);

        IEnumerable<User> InvoiceUsers(Garage garage);
    }
}
