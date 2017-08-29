namespace TheGarage.Services.AutomaticPayment.Core.Executions.Contracts
{
    using TheGarage.Data.Models;

    public interface IInvoiceEventExecutionEngine
    {
        void Execute(Garage garage);
    }
}