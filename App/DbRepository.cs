using System.Collections.Generic;
using System.Linq;
using App.SerializedClasses;
using Domain;
using Infrastructure;

namespace App
{
    public class DbRepository : IRepository
    {
        private readonly IDatabase<DbCard> cardDatabase;
        private readonly IDatabase<DbDeck> deckDatabase;

        public DbRepository(IDatabase<DbCard> cardDatabase, IDatabase<DbDeck> deckDatabase)
        {
            this.cardDatabase = cardDatabase;
            this.deckDatabase = deckDatabase;
        }

        public void SaveCard(Card card)
        {
            cardDatabase.Save(new DbCard(card));
        }

        public DbCard GetCard(string cardId)
        {
            var dbCard = cardDatabase.Get(cardId);
            return dbCard;
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

        public DbDeck GetDeck(string deckId)
        {
            var dbDeck = deckDatabase.Get(deckId);
            return dbDeck;
        }

        public void DeleteDeck(string deckId)
        {
            foreach (var card in GetCardsByDeckId(deckId))
                cardDatabase.Delete(card.Id);
            deckDatabase.Delete(deckId);
        }

        public IEnumerable<DbDeck> GetDecksByUser(User user)
        {
            return deckDatabase.GetAll(d => d.UserId == user.Id);
        }

        public IEnumerable<DbCard> GetCardsByDeckId(string deckId)
        {
            return cardDatabase.GetAll(c => c.DeckId == deckId);
        }

        public IEnumerable<string> GetDecksNamesByUser(User user)
        {
            return deckDatabase.GetAll(d => d.UserId == user.Id)
                .Select(d => d.Name);
        }
    }
}