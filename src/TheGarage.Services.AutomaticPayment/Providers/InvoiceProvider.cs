namespace TheGarage.Services.AutomaticPayment.Providers
{
    using System;
    using System.Collections.Generic;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Factories.Contracts;
    using TheGarage.Services.AutomaticPayment.Logic.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    public class InvoiceProvider : IInvoiceProvider
    {
        private readonly IModelsFactory modelsFactory;
        private readonly IInvoiceNumberGenerator invoiceNumberGenerator;
        private readonly IInvoiceAmountCalculator invoiceAmountCalculator;

        public InvoiceProvider(IModelsFactory modelsFactory, IInvoiceNumberGenerator invoiceNumberGenerator, IInvoiceAmountCalculator invoiceAmountCalculator)
        {
            this.modelsFactory = modelsFactory;
            this.invoiceNumberGenerator = invoiceNumberGenerator;
            this.invoiceAmountCalculator = invoiceAmountCalculator;
        }

        public Invoice Create(IEnumerable<Card> cards,  Garage garage, Company company, User user)
        {
            if (garage.CompanyId != company.Id)
            {
                throw new ArgumentException("That is not allowed");
            }

            var amount = this.invoiceAmountCalculator.TotalAmount(cards, garage.TAX);
            var number = this.invoiceNumberGenerator.Generate(company.SequenceNumber, garage.UniqueNumber, garage.SequenceInvoiceNumber);

            var subscriber = string.Empty;

            if (user.IsCompany)
            {
                if (user.CustomerCompany == null)
                {
                    throw new ArgumentException("Customer Company entity is required for this user");
                }

                subscriber = user.CustomerCompany.Name;
            }
            else
            {
                if (user.Customer == null)
                {
                    throw new ArgumentException("Customer entity is required for this user");
                }

                subscriber = user.Customer.FirstName + " " + user.Customer.LastName;
            }

            return this.modelsFactory.CreateMonthlyInvoiceBySystem(garage.Id, garage.Name, user.Id, subscriber, number, amount);
        }
    }
}
