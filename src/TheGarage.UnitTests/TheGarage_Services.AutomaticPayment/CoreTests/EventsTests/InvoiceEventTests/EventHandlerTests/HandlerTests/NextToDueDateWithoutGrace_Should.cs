namespace TheGarage.UnitTests.TheGarage_Services.AutomaticPayment.CoreTests.EventsTests.InvoiceEventTests.EventHandlerTests.HandlerTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TheGarage.Services.AutomaticPayment.Core.Events.EventHandler;
    using TheGarage.Services.AutomaticPayment.Providers;

    [TestClass]
    public class NextToDueDateWithoutGrace_Should
    {
        [TestMethod]
        public void ReturnTrue_WhenTodayIsNextDayToDueDateWithoutGracePeriod()
        {
            // Arrange
            var returnDate = new DateTime(2017, 6, 2, 15, 00, 00);

            var dateTimeMock = new Mock<DateTimeProvider>();

            dateTimeMock.SetupGet(x => x.Now).Returns(returnDate);

            DateTimeProvider.Current = dateTimeMock.Object;

            var handler = new Handler(1, 6, 0);

            // Act
            var isNextToDueDateWithoutGrace = handler.CheckNextToDueDateWithoutGrace();

            // Assert
            Assert.IsTrue(isNextToDueDateWithoutGrace);
        }
    }
}
