using System;
using System.Collections.Generic;
using App.SerializedClasses;
using Infrastructure;

namespace App
{
    public class DeckRepository : IRepository<DbDeck>
    {
        private readonly IDatabase<DbDeck> deckDatabase;

        public DeckRepository(IDatabase<DbDeck> deckDatabase)
        {
            this.deckDatabase = deckDatabase;
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
        }

        public IEnumerable<DbDeck> Search(Func<DbDeck, bool> filter)
        {
            return deckDatabase.GetAll(filter);
        }
    }
}