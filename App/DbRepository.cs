using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            var dbCard = cardDatabase.Get(cardId);
            return ConvertDbCardToCard(dbCard);
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
            return ConvertDbDeckToDeck(dbDeck, GetCardsByDeckId(deckId));
        }

        public void DeleteDeck(string deckId)
        {
            foreach (var card in GetDeck(deckId).Cards)
                cardDatabase.Delete(card.Id.ToString());
            deckDatabase.Delete(deckId);
        }

        public IEnumerable<string> GetDecksNamesByUser(User user)
        {
            return deckDatabase.GetAll()
                .Where(d => d.UserId == user.Id)
                .Select(d => d.Name);
        }

        public IEnumerable<Deck> GetDecksByUser(User user)
        {
            return deckDatabase.GetAll()
                .Where(d => d.UserId == user.Id)
                .Select(d => ConvertDbDeckToDeck(d, GetCardsByDeckId(d.Id)));
        }

        private Deck ConvertDbDeckToDeck(DbDeck dbDeck, IEnumerable<Card> cards)
        {
            var method = learnMethods.FirstOrDefault(m => m.Name == dbDeck.LearnMethod);
            return new Deck(Guid.Parse(dbDeck.Id), new User(dbDeck.UserId), dbDeck.Name, method, cards);
        }

        private static Card ConvertDbCardToCard(DbCard dbCard)
        {
            var parameters = JsonConvert.DeserializeObject<IParameters>(dbCard.Parameters);
            return new Card(Guid.Parse(dbCard.Id), new User(dbCard.UserId), Guid.Parse(dbCard.DeckId), dbCard.Front,
                dbCard.Back,
                TimeSpan.Parse(dbCard.TimeBeforeLearn),
                DateTime.Parse(dbCard.LastLearnTime, CultureInfo.InvariantCulture), parameters);
        }

        private IEnumerable<Card> GetCardsByDeckId(string deckId)
        {
            return cardDatabase.GetAll()
                .Where(c => c.DeckId == deckId)
                .Select(ConvertDbCardToCard);
        }
    }
}