namespace TheGarage.Services.AutomaticPayment.Core.Events.Contracts
{
    using TheGarage.Data;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts;

    public interface IPaymentHandlerProcesor
    {
        void ProcessPayment(IHandler handler, Garage garage);
    }
}