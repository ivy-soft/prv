namespace TheGarage.Services.AutomaticPayment.Core.Events
{
    using System;
    using TheGarage.Data;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts;

    public class DueDateEvent : AutomaticPaymentBase
    {
        protected override bool CanHandle(IHandler handler)
        {
            throw new NotImplementedException();
        }

        protected override void ProccessPaymentInternal(IHandler handler, Garage garage)
        {
            throw new NotImplementedException();
        }
    }
}