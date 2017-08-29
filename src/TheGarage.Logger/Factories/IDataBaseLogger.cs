namespace TheGarage.Logger.Factories
{
    using TheGarage.Data.Models;

    public interface IDataBaseLogger
    {
        Log CreateDataBaseLogger(string userEmail, string companyName, string garageName, string exception);
    }
}
