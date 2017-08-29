namespace TheGarage.Services.AutomaticPayment.Core.Events
{
    using System;
    using TheGarage.Data.Models;
    using TheGarage.Logger.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Executions.Contracts;

    public class InvoiceEvent : AutomaticPaymentBase
    {
        private readonly Lazy<IElmahLoggerSystem> elmagLoggerSystem;
        private readonly IInvoiceEventExecutionEngine invoiceEventExecutionEngine;

        public InvoiceEvent(Lazy<IElmahLoggerSystem> elmagLoggerSystem, IInvoiceEventExecutionEngine invoiceEventExecutionEngine/*ITheGarageData data, IInvoiceEventFacade invoiceEventFacade, IPaymentUser paymentUser, ICardProvider cardProvider, IEmailMessageService emailMessageService, IEmailTemplateProvider emailTemplateProvider, IDataBaseLogger databaseLogger*/)
        {
            this.elmagLoggerSystem = elmagLoggerSystem;
            this.invoiceEventExecutionEngine = invoiceEventExecutionEngine;
        }

        protected override bool CanHandle(IHandler handler)
        {

            return handler.CheckInvoiceDate();
        }

        protected override void ProccessPaymentInternal(IHandler handler, Garage garage)
        {
            try
            {
                this.invoiceEventExecutionEngine.Execute(garage);
            }
            catch (Exception ex)
            {
                this.elmagLoggerSystem.Value.Log(ex);
            }
        }
    }
}