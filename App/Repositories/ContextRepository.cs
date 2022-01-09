using System;
using System.Collections.Generic;
using App.SerializedClasses;
using Infrastructure;

namespace App
{
    public class ContextRepository : IRepository<DbContext>
    {
        private readonly IDatabase<DbContext> contextDatabase;

        public ContextRepository(IDatabase<DbContext> contextDatabase)
        {
            this.contextDatabase = contextDatabase;
        }

        public void Save(DbContext context)
        {
            contextDatabase.Save(context);
        }

        public DbContext Get(string userId)
        {
            return contextDatabase.Get(userId);
        }

        public void Update(DbContext context)
        {
            contextDatabase.Delete(context.UserId);
            contextDatabase.Save(context);
        }

        public void Delete(string userId)
        {
            contextDatabase.Delete(userId);
        }

        public IEnumerable<DbContext> Search(Func<DbContext, bool> filter)
        {
            return contextDatabase.GetAll(filter);
        }
    }
}