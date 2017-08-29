namespace TheGarage.AutomaticPayment.Client
{
    using Ninject;
    using NinjectIoCContainer.Configuration;
    using TheGarage.Services.AutomaticPayment.Contracts.Core.Contracts;

    public class PaymentTask
    {
        public static void Main()
        {
            var kernel = new StandardKernel(new TheGarageModule());
            var engine = kernel.Get<IEngine>();
            engine.Start();
        }
    }
}

