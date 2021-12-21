using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IDatabase<T>
    {
        void CreateTable(string connectionString);
        void Save(T item);
        T Get(string id);
        void Delete(string id);
        IEnumerable<T> GetAll(Func<T, bool> filter);
    }
}