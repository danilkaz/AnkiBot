using System;
using System.Collections.Generic;
using System.Linq;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Domain.Parameters;
using AnkiBot.Infrastructure;
using App.SerializedClasses;
using Newtonsoft.Json;

namespace App
{
    public class DbRepository : IRepository
    {
        private readonly IDatabase<DbCard> cardDatabase;
        private readonly IDatabase<DbDeck> deckDatabase;
        private readonly ILearnMethod[] learnMethods;

        public DbRepository(IDatabase<DbCard> cardDatabase, IDatabase<DbDeck> deckDatabase, ILearnMethod[] learnMethods)
        {
            this.cardDatabase = cardDatabase;
            this.deckDatabase = deckDatabase;
            this.learnMethods = learnMethods;
        }

        public void SaveCard(Card card)
        {
            cardDatabase.Save(new DbCard(card));
        }

        public Card GetCard(string cardId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCard(Card card)
        {
            DeleteCard(card.Id.ToString());
            SaveCard(card);
        }

        public void DeleteCard(string cardId)
        {
            cardDatabase.Delete(cardId);
        }

        public void SaveDeck(Deck deck)
        {
            deckDatabase.Save(new DbDeck(deck));
        }

        public Deck GetDeck(string deckId)
        {
            var dbDeck = deckDatabase.Get(deckId);
            return ConvertDbDeckToDeck(dbDeck);
        }

        public void DeleteDeck(string deckId)
        {
            foreach (var card in GetCardsByDeckId(deckId))
                cardDatabase.Delete(card.Id.ToString());
            deckDatabase.Delete(deckId);
        }

        public IEnumerable<Deck> GetDecksByUser(User user)
        {
            return deckDatabase.GetAll()
                .Where(d => d.UserId == user.Id)
                .Select(ConvertDbDeckToDeck);
        }

        public IEnumerable<Card> GetCardsByDeckId(string deckId)
        {
            return cardDatabase.GetAll()
                .Where(c => c.DeckId.ToString() == deckId)
                .Select(ConvertDbCardToDeck);
        }

        public IEnumerable<Card> GetCardsToLearn(string deckId)
        {
            return GetCardsByDeckId(deckId)
                .Where(c => c.NextLearnTime < DateTime.Now)
                .OrderBy(c => c.NextLearnTime);
        }

        private Deck ConvertDbDeckToDeck(DbDeck dbDeck)
        {
            var method = learnMethods.FirstOrDefault(m => m.Name == dbDeck.LearnMethod);
            return new Deck(dbDeck.Id, new User(dbDeck.UserId), dbDeck.Name, method);
        }

        private static Card ConvertDbCardToDeck(DbCard dbCard)
        {
            var parameters = JsonConvert.DeserializeObject<IParameters>(dbCard.Parameters);
            return new Card(dbCard.Id, new User(dbCard.UserId), dbCard.DeckId, dbCard.Front, dbCard.Back,
                TimeSpan.Parse(dbCard.TimeBeforeLearn), DateTime.Parse(dbCard.LastLearnTime), parameters);
        }
    }
}