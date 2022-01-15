using System;
using System.Collections.Generic;
using App.SerializedClasses;
using Infrastructure;

namespace App
{
    public class DeckRepository : IRepository<DbDeck>
    {
        private readonly IDatabase<DbCard> cardDatabase;
        private readonly IDatabase<DbDeck> deckDatabase;
        private readonly IDatabase<DbCard> cardDatabase;

        public DeckRepository(IDatabase<DbDeck> deckDatabase, IDatabase<DbCard> cardDatabase)
        {
            this.deckDatabase = deckDatabase;
            this.cardDatabase = cardDatabase;
        }

        public void Save(DbDeck deck)
        {
            deckDatabase.Save(deck);
        }

        public DbDeck Get(string id)
        {
            return deckDatabase.Get(id);
        }

        public void Update(DbDeck deck)
        {
            Delete(deck.Id);
            Save(deck);
        }

        public void Delete(string id)
        {
            deckDatabase.Delete(id);
            foreach (var card in cardDatabase.GetAll(c => c.DeckId == id))
                cardDatabase.Delete(card.Id);
        }

        public IEnumerable<DbDeck> Search(Func<DbDeck, bool> filter)
        {
            return deckDatabase.GetAll(filter);
        }
    }
}