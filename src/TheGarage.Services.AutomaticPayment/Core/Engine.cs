namespace TheGarage.Services.AutomaticPayment.Core
{
    using System;
    using TheGarage.Data;
    using TheGarage.Logger.Contracts;
    using TheGarage.Services.AutomaticPayment.Contracts.Core.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.Contracts;
    using TheGarage.Services.AutomaticPayment.Factories.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class Engine : IEngine
    {
        private readonly IHandlerFactory handlerFactory;
        private readonly IPaymentHandler paymentHandler;
        private readonly ITheGarageData data;
        private readonly IDataProvider dataProvider;
        private readonly Lazy<IElmahLoggerSystem> elmagLoggerSystem;

        public Engine(Lazy<IElmahLoggerSystem> elmagLoggerSystem, IHandlerFactory handlerFactory, IPaymentHandler paymentHandler, ITheGarageData data, IDataProvider dataProvider /*, IPaymentEvent paymentEvent, ICardProvider cardProvider, IPaymentUser paymentUser, IInvoiceProvider invoiceProvider, IInvoiceDetailsProvider invoiceDetailsProvider, IGarageProvider garageProvider, IUserProvider userProvider, IEmailProvider emailProvider*/)
        {
            this.elmagLoggerSystem = elmagLoggerSystem;
            this.handlerFactory = handlerFactory;
            this.paymentHandler = paymentHandler;
            this.data = data;
            this.dataProvider = dataProvider;
        }

        public void Start()
        {
            var allGarages = this.dataProvider.GetAllGarages();

            foreach (var garage in allGarages)
            {
                var handler = this.handlerFactory.CreateHandler(garage.DueDate, garage.InDaysInvoiceEventBeforeDueDate, garage.GracePeriod);

                this.paymentHandler.ProcessPayment(handler, garage);
            }
        }
    }
}