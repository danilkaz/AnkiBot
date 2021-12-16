using System;
using System.Collections.Generic;

namespace AnkiBot.Infrastructure
{
    public interface IDatabase<T>
    {
        void Save(T item);
        T Get(string id);
        void Delete(string id);
        IEnumerable<T> GetAll();
    }
}