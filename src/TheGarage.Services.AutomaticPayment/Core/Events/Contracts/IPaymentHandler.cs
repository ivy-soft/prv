namespace TheGarage.Services.AutomaticPayment.Core.Events.Contracts
{
    public interface IPaymentHandler : IPaymentHandlerProcesor
    {
        void SetSuccessor(IPaymentHandlerProcesor paymentHandler);
    }
}
