namespace TheGarage.UnitTests.TheGarage_Services.AutomaticPayment.CoreTests.ExecutionsTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TheGarage.Data;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Core.Executions;
    using TheGarage.Services.AutomaticPayment.Facade.Contracts;
    using TheGarage.Services.AutomaticPayment.Providers.Contracts;

    [TestClass]
    public class Execute_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_WhenGarageIsNull()
        {
            // Arrange
            var dataMock = new Mock<ITheGarageData>();
            var invoiceEventFacadeMock = new Mock<IInvoiceEventFacade>();
            var dataProviderMock = new Mock<IDataProvider>();
            var cardProviderMock = new Mock<ICardProvider>();
            var emailMessageFacadeMock = new Mock<IEmailMessageFacade>();

            var invoiceEventExecutionEngine = new InvoiceEventExecutionEngine(dataMock.Object, invoiceEventFacadeMock.Object, dataProviderMock.Object, cardProviderMock.Object, emailMessageFacadeMock.Object);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => invoiceEventExecutionEngine.Execute(null));
        }

        [TestMethod]
        public void ThrowArgumentNullException_WhenGarageCompanyIsNull()
        {
            // Arrange
            var dataMock = new Mock<ITheGarageData>();
            var invoiceEventFacadeMock = new Mock<IInvoiceEventFacade>();
            var dataProviderMock = new Mock<IDataProvider>();
            var cardProviderMock = new Mock<ICardProvider>();
            var emailMessageFacadeMock = new Mock<IEmailMessageFacade>();

            var invoiceEventExecutionEngine = new InvoiceEventExecutionEngine(dataMock.Object, invoiceEventFacadeMock.Object, dataProviderMock.Object, cardProviderMock.Object, emailMessageFacadeMock.Object);

            var garage = new Garage();
    
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => invoiceEventExecutionEngine.Execute(garage));
        }

        [TestMethod]
        public void ssss_WhenGarageCompanyIsNull()
        {
            // Arrange
            var dataMock = new Mock<ITheGarageData>();
            var invoiceEventFacadeMock = new Mock<IInvoiceEventFacade>();
            var dataProviderMock = new Mock<IDataProvider>();
            var cardProviderMock = new Mock<ICardProvider>();
            var emailMessageFacadeMock = new Mock<IEmailMessageFacade>();

            var invoiceEventExecutionEngine = new InvoiceEventExecutionEngine(dataMock.Object, invoiceEventFacadeMock.Object, dataProviderMock.Object, cardProviderMock.Object, emailMessageFacadeMock.Object);

            var garage = new Garage();
            var company = new Company();
            garage.Company = company;

            var user = new User
            {
                PaidTrough = new DateTime(2017, 8, 29),
                AutoCharge = true
            };

            var card = new Card();
            user.Cards.Add(card);
            var userCards = user.Cards.ToList();
            var userCollection = new List<User>();
            userCollection.Add(user);

            dataProviderMock.Setup(x => x.InvoiceUsers(garage)).Returns(userCollection);

            cardProviderMock.Setup(x => x.FilteringValidCardsForInvoice(userCards)).Returns(userCards);

            var invoice = new Invoice();
            invoice.Amount = 111;
            invoiceEventFacadeMock.Setup(x => x.CreateInvoice(userCards, garage, garage.Company, user)).Returns(invoice);
            dataMock.Setup(x => x.Invoices.Add(invoice));

            invoiceEventFacadeMock.Setup(x => x.UpdateMonthlyInvoiceData(garage)).Returns(garage);
            dataMock.Setup(x => x.Garages.Update(garage));

            invoiceEventFacadeMock.Setup(x => x.UpdateMonthlyInvoiceData(user, invoice.Amount)).Returns(user);
            dataMock.Setup(x => x.Users.Update(user));


            // Act
            invoiceEventExecutionEngine.Execute(garage);

            dataMock.Verify(x => x.Invoices.Add(invoice), Times.Once);
        }
    }
}
