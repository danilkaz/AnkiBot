using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Infrastructure;
using App.SerializedClasses;

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

        public void SaveDeck(Deck deck)
        {
            _deckDatabase.Save(new DbDeck(deck));
        }

        public Deck GetDeck(string deckId)
        {
            var dbDeck = _deckDatabase.Get(deckId);
            return ConvertDbDeckToDeck(dbDeck);
        }

        public IEnumerable<Deck> GetDecksByUserId(string userId)
        {
            return _deckDatabase.GetAll()
                .Where(d => d.UserId == userId)
                .Select(ConvertDbDeckToDeck);
        }

        public IEnumerable<Card> GetCardsByDeckId(string deckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Card> GetCardsToLearn(string deckId)
        {
            throw new NotImplementedException();
        }

        private Deck ConvertDbDeckToDeck(DbDeck dbDeck)
        {
            var method = _learnMethods.FirstOrDefault(m => m.Name == dbDeck.LearnMethod);
            return new Deck(dbDeck.Id, dbDeck.Name, method);
        }
    }
}
