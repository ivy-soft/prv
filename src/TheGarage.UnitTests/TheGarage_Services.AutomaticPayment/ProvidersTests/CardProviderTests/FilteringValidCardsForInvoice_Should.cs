namespace TheGarage.UnitTests.TheGarage_Services.AutomaticPayment.ProvidersTests.CardProviderTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TheGarage.Data.Models;
    using TheGarage.Services.AutomaticPayment.Providers;

    [TestClass]
    public class FilteringValidCardsForInvoice_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_WhenPassedParameterIsNull()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new CardProvider().FilteringValidCardsForInvoice(null));
        }

        [TestMethod]
        public void ReturnEmptyCollection_WhenCardsIsDeleted()
        {
            // Arrange
            var dateTimeProviderMock = new Mock<DateTimeProvider>();

            var date = new DateTime(2017, 8, 1, 00, 00, 00);

            dateTimeProviderMock.SetupGet(x => x.Now).Returns(date);

            DateTimeProvider.Current = dateTimeProviderMock.Object;

            var cardCollection = new List<Card>();
            var card = new Card();

            card.IsActive = true;
            card.IsDeleted = true;
            card.ValidFrom = DateTimeProvider.Current.Now.AddMonths(-1);

            cardCollection.Add(card);

            var cardProvider = new CardProvider();

            // Act
            var filteredCards = cardProvider.FilteringValidCardsForInvoice(cardCollection).ToList();

            // Assert
            Assert.AreEqual(0, filteredCards.Count);
        }

        [TestMethod]
        public void ReturnEmptyCollection_WhenCardsIsNotActive()
        {
            // Arrange
            var dateTimeProviderMock = new Mock<DateTimeProvider>();

            var date = new DateTime(2017, 8, 1, 00, 00, 00);

            dateTimeProviderMock.SetupGet(x => x.Now).Returns(date);

            DateTimeProvider.Current = dateTimeProviderMock.Object;

            var cardCollection = new List<Card>();
            var card = new Card();

            card.IsActive = false;
            card.IsDeleted = false;
            card.ValidFrom = DateTimeProvider.Current.Now.AddMonths(-1);

            cardCollection.Add(card);

            var cardProvider = new CardProvider();

            // Act
            var filteredCards = cardProvider.FilteringValidCardsForInvoice(cardCollection).ToList();

            // Assert
            Assert.AreEqual(0, filteredCards.Count);
        }

        [TestMethod]
        public void ReturnEmptyCollection_WhenCardsValidFromMonthIsNotLessOrEqualOnCurrentMonth()
        {
            // Arrange
            var dateTimeProviderMock = new Mock<DateTimeProvider>();

            var date = new DateTime(2017, 8, 1, 00, 00, 00);

            dateTimeProviderMock.SetupGet(x => x.Now).Returns(date);

            DateTimeProvider.Current = dateTimeProviderMock.Object;

            var cardCollection = new List<Card>();
            var card = new Card();

            card.IsActive = true;
            card.IsDeleted = false;
            card.ValidFrom = DateTimeProvider.Current.Now.AddMonths(1);

            cardCollection.Add(card);

            var cardProvider = new CardProvider();

            // Act
            var filteredCards = cardProvider.FilteringValidCardsForInvoice(cardCollection).ToList();

            // Assert
            Assert.AreEqual(0, filteredCards.Count);
        }

        [TestCleanup]
        public void ResetDateTimeProvider()
        {
            DateTimeProvider.ResetToDefault();
        }
    }
}
