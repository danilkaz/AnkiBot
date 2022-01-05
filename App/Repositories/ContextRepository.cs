using System;
using System.Collections.Generic;
using App.SerializedClasses;
using Infrastructure;

namespace App
{
    public class ContextRepository : IRepository<DbContext>
    {
        private readonly IDatabase<DbContext> stateDatabase;

        public ContextRepository(IDatabase<DbContext> stateDatabase)
        {
            this.stateDatabase = stateDatabase;
        }

        public void Save(DbContext context)
        {
            stateDatabase.Save(context);
        }

        public DbContext Get(string userId)
        {
            return stateDatabase.Get(userId);
        }

        public void Update(DbContext context)
        {
            stateDatabase.Delete(context.UserId);
            stateDatabase.Save(context);
        }

        public void Delete(string userId)
        {
            stateDatabase.Delete(userId);
        }

        public IEnumerable<DbContext> Search(Func<DbContext, bool> filter)
        {
            return stateDatabase.GetAll(filter);
        }
    }
}