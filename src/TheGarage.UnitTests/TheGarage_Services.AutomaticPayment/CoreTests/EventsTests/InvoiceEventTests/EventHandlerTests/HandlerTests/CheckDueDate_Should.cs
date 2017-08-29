namespace TheGarage.UnitTests.TheGarage_Services.AutomaticPayment.CoreTests.EventsTests.InvoiceEventTests.EventHandlerTests.HandlerTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler;
    using TheGarage.Services.AutomaticPayment.Providers;

    [TestClass]
    public class CheckDueDate_Should
    {
        [TestMethod]
        public void ReturnTrue_WhenTodayIsEqualOnDueDate()
        {
            // Arrange
            var returnDate = new DateTime(2017, 6, 1, 15, 00, 00);

            var dateTimeMock = new Mock<DateTimeProvider>();

            dateTimeMock.SetupGet(x => x.Now).Returns(returnDate);

            DateTimeProvider.Current = dateTimeMock.Object;

            var handler = new Handler(1, 6, 0);

            // Act
            var isDueDate = handler.CheckDueDate();

            // Assert
            Assert.IsTrue(isDueDate);
        }
    }
}
