namespace TheGarage.UnitTests.TheGarage_Services.AutomaticPayment.DateTimeProviderTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TheGarage.Services.AutomaticPayment.Providers;

    [TestClass]
    public class Now_Should
    {
        [TestMethod]
        public void ValidateServerTimeIsCultureEnUS()
        {
            // Arrange
            var cultureUS = new System.Globalization.CultureInfo("en-US");

            var returnDate = new DateTime(2017, 6, 15, 15, 00, 00);
            var dateTimeMock = new Mock<DateTimeProvider>();
            dateTimeMock.SetupGet(x => x.Now).Returns(returnDate);
            DateTimeProvider.Current = dateTimeMock.Object;

            // Assert
            Assert.AreEqual(returnDate.ToString(cultureUS), DateTimeProvider.Current.Now.ToString());
        }

        [TestMethod]
        public void ValidateDatetimeNowEqualAtDatetimeProviderNow()
        {
            // Arrange, Act, Assert
            Assert.AreEqual(DateTime.Now.ToString(), DateTimeProvider.Current.Now.ToString());
        }

        [TestCleanup]
        public void ResetDateTimeProvider()
        {
            DateTimeProvider.ResetToDefault();
        }
    }
}
