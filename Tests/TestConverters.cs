using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using App;
using App.Converters;
using App.SerializedClasses;
using App.UIClasses;
using Domain;
using Domain.LearnMethods;
using Domain.Parameters;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    public class TestConverters
    {
        private CardConverter cardConverter;
        private DeckConverter deckConverter;

        [SetUp]
        public void Setup()
        {
            cardConverter = new();
            
            var cardRepository = new Mock<IRepository<DbCard>>();
            cardRepository.Setup(c => c.Search(It.IsAny<Func<DbCard, bool>>())).Returns(new List<DbCard>());
            deckConverter = new(new ILearnMethod[] { new LineLearnMethod() }, cardRepository.Object, cardConverter);
        }

        [Test]
        public void TestCardConverterToUi()
        {
            var dbCard = new DbCard("id", "front", "back", "userId", "deckId", "", "", "");
            var uiCard = cardConverter.ToUi(dbCard);
            var excepted = new UICard("id", "front", "back", "deckId");
            Assert.IsTrue(uiCard.GetType() == excepted.GetType());
            Assert.IsTrue(uiCard.Id == excepted.Id);
            Assert.IsTrue(uiCard.Front == excepted.Front);
            Assert.IsTrue(uiCard.Back == excepted.Back);
            Assert.IsTrue(uiCard.DeckId == excepted.DeckId);
        }
        
        [Test]
        public void TestCardConverterToDomainClass()
        {
            var id = Guid.NewGuid();
            var deckId = Guid.NewGuid();
            var timeBeforeLearn = new TimeSpan(1, 0, 0, 0, 0);
            var lastLearnTime = DateTime.Now;
            var dbCard = new DbCard(id.ToString(), "front", "back", "1", deckId.ToString(), timeBeforeLearn.ToString(),
                lastLearnTime.ToString(CultureInfo.InvariantCulture), JsonConvert.SerializeObject(new EmptyParameters()));
            var domainCard = cardConverter.ToDomainClass(dbCard);
            var excepted = new Card(id, new User("1"), deckId, "front", "back", timeBeforeLearn, lastLearnTime,
                new EmptyParameters());
            Assert.IsTrue(domainCard.GetType() == excepted.GetType());
            Assert.IsTrue(domainCard.Front == excepted.Front);
            Assert.IsTrue(domainCard.Back == excepted.Back);
            Assert.IsTrue(domainCard.Id == excepted.Id);
            Assert.IsTrue(domainCard.DeckId == excepted.DeckId);
            Assert.IsTrue(domainCard.User == excepted.User);
            Assert.IsTrue(domainCard.Parameters.Equals(excepted.Parameters));
        }

        [Test]
        public void TestDeckConverterToUi()
        {
            var dbDeck = new DbDeck("id", "userId", "name", "learnMethod");
            var uiDeck = deckConverter.ToUi(dbDeck);
            var excepted = new UIDeck("id", "name", "learnMethod");
            Assert.IsTrue(uiDeck.GetType() == excepted.GetType());
            Assert.IsTrue(uiDeck.Id == excepted.Id);
            Assert.IsTrue(uiDeck.Name == excepted.Name);
            Assert.IsTrue(uiDeck.LearnMethod == excepted.LearnMethod);
        }
        
        [Test]
        public void TestDeckConverterToDomainClass()
        {
            var id = Guid.NewGuid();
            var learnMethod = new LineLearnMethod();
            var dbDeck = new DbDeck(id.ToString(), "1", "name", learnMethod.Name);
            var domainDeck = deckConverter.ToDomainClass(dbDeck);
            var excepted = new Deck(id, new User("1"), "name", learnMethod, new List<Card>());
            Assert.IsTrue(domainDeck.GetType() == excepted.GetType());
            Assert.IsTrue(domainDeck.Id == excepted.Id);
            Assert.IsTrue(domainDeck.Name == excepted.Name);
            Assert.IsTrue(domainDeck.User == excepted.User);
            Assert.IsTrue(domainDeck.Cards.Count() == excepted.Cards.Count());
            Assert.IsTrue(domainDeck.LearnMethod.Name == excepted.LearnMethod.Name);
        }
    }
}