namespace TheGarage.Services.AutomaticPayment.Facade.Contracts
{
    using TheGarage.Data.Models;

    public interface IEmailMessageFacade
    {
        EmailMessage Send(User user, Garage garage, Invoice invoice);
    }
}
