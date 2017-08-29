namespace TheGarage.UnitTests.TheGarage_Services.AutomaticPayment.CoreTests.EventsTests.InvoiceEventTests.EventHandlerTests.HandlerTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler;
    using TheGarage.Services.AutomaticPayment.Providers;

    [TestClass]
    public class LastPaymentDay_Should
    {
        [TestMethod]
        public void ReturnTrue_WhenTodayIsEqualOnDueDatePlusGracePeriod()
        {
            // Arrange
            var returnDate = new DateTime(2017, 6, 4, 15, 00, 00);

            var dateTimeMock = new Mock<DateTimeProvider>();

            dateTimeMock.SetupGet(x => x.Now).Returns(returnDate);

            DateTimeProvider.Current = dateTimeMock.Object;

            var handler = new Handler(1, 6, 2);

            // Act
            var isLastPaymentDay = handler.CheckLastPaymentDay();

            // Assert
            Assert.IsTrue(isLastPaymentDay);
        }
    }
}
