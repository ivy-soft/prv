namespace TheGarage.Services.AutomaticPayment.Factories.Contracts
{
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts;

    public interface IHandlerFactory
    {
        IHandler CreateHandler(int dueDate, int invoiceEvent, int gracePeriod);
    }
}
