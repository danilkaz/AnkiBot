using System;
using System.Collections;
using System.Collections.Generic;

namespace AnkiBot.Infrastructure
{
    public interface IDatabase<T>
    {
        void Save(T item);
        T Get(string id);
        void Delete(string id);
        IEnumerable<T> Where(Func<T, bool> filter);
        IEnumerable<T> GetAll();
    }
}