using System;
using System.Collections.Generic;
using App.SerializedClasses;
using Infrastructure;

namespace App
{
    public class CardRepository : IRepository<DbCard>
    {
        private readonly IDatabase<DbCard> cardDatabase;

        public CardRepository(IDatabase<DbCard> cardDatabase)
        {
            this.cardDatabase = cardDatabase;
        }

        public void Save(DbCard card)
        {
            cardDatabase.Save(card);
        }

        public DbCard Get(string id)
        {
            return cardDatabase.Get(id);
        }

        public void Update(DbCard card)
        {
            Delete(card.Id);
            Save(card);
        }

        public void Delete(string id)
        {
            cardDatabase.Delete(id);
        }

        public IEnumerable<DbCard> Search(Func<DbCard, bool> filter)
        {
            return cardDatabase.GetAll(filter);
        }
    }
}