namespace NinjectIoCContainer.Configuration
{
    using System.Data.Entity;
    using Ninject;
    using Ninject.Extensions.Factory;
    using Ninject.Modules;
    using TheGarage.Data;
    using TheGarage.Data.Common.Repositories;
    using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Contracts;
    using TheGarage.Services.AutomaticPayment.EmailMessageTemplates.Factories.Contracts;
    using TheGarage.Services.AutomaticPayment.Factories;
    using TheGarage.Services.AutomaticPayment.Factories.Contracts;
    using TheGarage.Services.AutomaticPayment.Logic;
    using TheGarage.Services.AutomaticPayment.Logic.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;
    using TheGarage.Services.AutomaticPayment.Facade;
    using TheGarage.Services.AutomaticPayment.Facade.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events;
    using TheGarage.Services.AutomaticPayment.Core.Events.Contracts;
    using TheGarage.Services.AutomaticPayment.Contracts.Core.Contracts;
    using TheGarage.Services.AutomaticPayment.Core;
    using TheGarage.Services.AutomaticPayment.Core.Executions;
    using TheGarage.Services.AutomaticPayment.Core.Executions.Contracts;
    using TheGarage.EmailService.Contracts;
    using TheGarage.EmailService.Settings.Contracts;
    using TheGarage.EmailService.Attachments.Contracts;
    using TheGarage.EmailService;
    using TheGarage.EmailService.Attachments;
    using TheGarage.EmailService.Models;
    using TheGarage.EmailService.Settings;
    using TheGarage.EmailService.Models.Contracts;
    using TheGarage.Logger;
    using TheGarage.Logger.Factories;
    using TheGarage.Logger.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler.Contracts;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler;

    public class TheGarageModule : NinjectModule
    {
        private const string InvoiceHeadHandlerName = "InvoiceEvent";
        private const string DueDateEventHandlerName = "DueDateEvent";
        private const string NextToDueDateWithoutGraceEventHandlerName = "NextToDueDateWithoutGraceEvent";
        private const string NextToDueDateWithGraceEventHandlerName = "NextToDueDateWithGraceEvent";
        private const string LastPaymentDayEventHandlerName = "LastPaymentDayEvent";

        public override void Load()
        {
            //this.Kernel.Bind(x =>
            //{
            //    x.FromAssembliesInPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            //    .SelectAllClasses()
            //    .Where(type => type != typeof(Engine) && type != typeof(TheGarageData) && type != typeof(DataProvider) && type != typeof(InvoiceEventFacade) && type != typeof(PaymentUser))
            //    .BindDefaultInterface();
            //});

            this.Bind<IEngine>().To<Engine>().InSingletonScope();

            this.Bind<DbContext>().To<TheGarageDbContext>().InSingletonScope();
            this.Bind<ITheGarageDbContext>().To<TheGarageDbContext>().InSingletonScope();
            this.Bind(typeof(IRepository<>)).To(typeof(EfGenericRepository<>)).InSingletonScope();
            this.Bind<ITheGarageData>().To<TheGarageData>().InSingletonScope(); // change to requestScope in web;

            this.Bind<IPaymentHandler>().To<InvoiceEvent>().InSingletonScope().Named(InvoiceHeadHandlerName);
            this.Bind<IPaymentHandler>().To<DueDateEvent>().InSingletonScope().Named(DueDateEventHandlerName);
            this.Bind<IPaymentHandler>().To<NextToDueDateWithoutGraceEvent>().InSingletonScope().Named(NextToDueDateWithoutGraceEventHandlerName);
            this.Bind<IPaymentHandler>().To<NextToDueDateWithGraceEvent>().InSingletonScope().Named(NextToDueDateWithGraceEventHandlerName);
            this.Bind<IPaymentHandler>().To<LastPaymentDayEvent>().InSingletonScope().Named(LastPaymentDayEventHandlerName);

            this.Bind<IPaymentHandler>().ToMethod(context =>
            {
                IPaymentHandler invoiceHeadHandler = context.Kernel.Get<IPaymentHandler>(InvoiceHeadHandlerName);
                IPaymentHandler dueDateEventHandler = context.Kernel.Get<IPaymentHandler>(DueDateEventHandlerName);
                IPaymentHandler nextToDueDateWithoutGraceEventHandler = context.Kernel.Get<IPaymentHandler>(NextToDueDateWithoutGraceEventHandlerName);
                IPaymentHandler nextToDueDateWithGraceEventHandler = context.Kernel.Get<IPaymentHandler>(NextToDueDateWithGraceEventHandlerName);
                IPaymentHandler lastPaymentDayEventHandler = context.Kernel.Get<IPaymentHandler>(LastPaymentDayEventHandlerName);

                invoiceHeadHandler.SetSuccessor(dueDateEventHandler);
                dueDateEventHandler.SetSuccessor(nextToDueDateWithoutGraceEventHandler);
                nextToDueDateWithoutGraceEventHandler.SetSuccessor(nextToDueDateWithGraceEventHandler);
                nextToDueDateWithGraceEventHandler.SetSuccessor(lastPaymentDayEventHandler);

                return invoiceHeadHandler;
            }).WhenInjectedInto<Engine>().InSingletonScope();

            this.Bind<IHandler>().To<Handler>();
            this.Bind<IInvoiceEventFacade>().To<InvoiceEventFacade>();
            this.Bind<IHandlerFactory>().ToFactory().InSingletonScope();
            this.Bind<IDataProvider>().To<DataProvider>().InSingletonScope();
            //this.Bind<IPaymentUser>().To<PaymentUser>().InSingletonScope();
            this.Bind<IInvoiceDetailsProvider>().To<InvoiceDetailsProvider>().InSingletonScope();
            this.Bind<IInvoiceProvider>().To<InvoiceProvider>().InSingletonScope();
            this.Bind<ICardProvider>().To<CardProvider>().InSingletonScope();
            this.Bind<IUserProvider>().To<UserProvider>().InSingletonScope();
            this.Bind<IGarageProvider>().To<GarageProvider>().InSingletonScope();
            this.Bind<IModelsFactory>().To<ModelsFactory>().InSingletonScope();
            this.Bind<IInvoiceNumberGenerator>().To<InvoiceNumberGenerator>().InSingletonScope();
            this.Bind<IInvoiceAmountCalculator>().To<InvoiceAmountCalculator>().InSingletonScope();
            this.Bind<IInvoiceEventExecutionEngine>().To<InvoiceEventExecutionEngine>().InSingletonScope();
            this.Bind<IElmahLoggerSystem>().To<ElmahLoggerSystem>().InSingletonScope();
            this.Bind<IEmailMessageFacade>().To<EmailMessageFacade>().InSingletonScope();
            
            this.Bind<IEmailMessageService>().To<EmailMessageService>();
            this.Bind<ISmtpSettings>().To<SmtpSettings>().InSingletonScope();
            this.Bind<ISmtpClientService>().To<SmtpClientService>().InSingletonScope();
            this.Bind<ISanitizeService>().To<SanitizeService>().InSingletonScope();
            this.Bind<IPdfAttachment>().To<PdfAttachment>().InSingletonScope();
            this.Bind<IMessageModel>().To<MessageModel>().InSingletonScope();

            this.Bind<IEmailTemplateProvider>().To<EmailTemplateProvider>().InSingletonScope();
            this.Bind<IEmailMessageTemplateFactory>().ToFactory().InSingletonScope();

            this.Bind<IDataBaseLogger>().ToFactory().InSingletonScope();
        }
    }
}
