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
        private readonly IDatabase<DbCard> _cardDatabase;
        private readonly IDatabase<DbDeck> _deckDatabase;
        private readonly ILearnMethod[] _learnMethods;

        public DbRepository(IDatabase<DbCard> cardDatabase, IDatabase<DbDeck> deckDatabase, ILearnMethod[] learnMethods)
        {
            _cardDatabase = cardDatabase;
            _deckDatabase = deckDatabase;
            _learnMethods = learnMethods;
        }

        public void SaveCard(Card card)
        {
            _cardDatabase.Save(new DbCard(card));
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
            _cardDatabase.Delete(cardId);
        }

        public void SaveDeck(Deck deck)
        {
            _deckDatabase.Save(new DbDeck(deck));
        }

        public Deck GetDeck(string deckId)
        {
            var dbDeck = _deckDatabase.Get(deckId);
            return ConvertDbDeckToDeck(dbDeck);
        }

        public void DeleteDeck(string deckId)
        {
            foreach (var card in GetCardsByDeckId(deckId))
                _cardDatabase.Delete(card.Id.ToString());
            _deckDatabase.Delete(deckId);
        }

        public IEnumerable<Deck> GetDecksByUserId(string userId)
        {
            return _deckDatabase.GetAll()
                .Where(d => d.UserId == userId)
                .Select(ConvertDbDeckToDeck);
        }

        public IEnumerable<Card> GetCardsByDeckId(string deckId)
        {
            return _cardDatabase.GetAll()
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
            var method = _learnMethods.FirstOrDefault(m => m.Name == dbDeck.LearnMethod);
            return new Deck(dbDeck.Id, dbDeck.UserId, dbDeck.Name, method);
        }

        private Card ConvertDbCardToDeck(DbCard dbCard)
        {
            var parameters = JsonConvert.DeserializeObject<IParameters>(dbCard.Parameters);
            return new Card(dbCard.Id, dbCard.UserId, dbCard.DeckId, dbCard.Front, dbCard.Back,
                TimeSpan.Parse(dbCard.TimeBeforeLearn), DateTime.Parse(dbCard.LastLearnTime), parameters);
        }
    }
}