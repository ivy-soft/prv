namespace TheGarage.Services.AutomaticPayment.Core.Events
{
    using Bytes2you.Validation;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts;

    public abstract class AutomaticPaymentBase : IPaymentHandler
    {
        private IPaymentHandlerProcesor Successor { get; set; }

        public void SetSuccessor(IPaymentHandlerProcesor paymentHandler)
        {
            Guard.WhenArgument(paymentHandler, "paymentHandler").IsNull().Throw();

            this.Successor = paymentHandler;
        }

        public void ProcessPayment(IHandler handler, Garage garage)
        {
            Guard.WhenArgument(handler, "handler").IsNull().Throw();
            Guard.WhenArgument(garage, "garage").IsNull().Throw();

            if (this.CanHandle(handler))
            {
                this.ProccessPaymentInternal(handler, garage);
            }
            else if (this.Successor != null)
            {
                this.Successor.ProcessPayment(handler, garage);
            }
            else
            {
                // Logger today miss event!!!
            }
        }

        protected abstract bool CanHandle(IHandler handler);

        protected abstract void ProccessPaymentInternal(IHandler handler, Garage garage);
    }
}
