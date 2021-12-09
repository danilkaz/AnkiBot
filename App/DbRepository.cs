using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Infrastructure;

namespace App
{
    public class DbRepository : IRepository
    {
        private readonly IDatabase<Card> _cardDatabase;
        private readonly IDatabase<Deck> _deckDatabase;

        public DbRepository(IDatabase<Card> cardDatabase, IDatabase<Deck> deckDatabase)
        {
            _cardDatabase = cardDatabase;
            _deckDatabase = deckDatabase;
        }
        public void SaveCard(Card card)
        {
            _cardDatabase.Save(card);
        }

        public Card GetCard(string cardId)
        {
            throw new NotImplementedException();
        }

        public void SaveDeck(Deck deck)
        {
            _deckDatabase.Save(deck);
        }

        public Deck GetDeck(string deckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Deck> GetDecksByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Card> GetCardsByDeckId(string deckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Card> GetCardsToLearn(string deckId)
        {
            throw new NotImplementedException();
        }
    }
}
