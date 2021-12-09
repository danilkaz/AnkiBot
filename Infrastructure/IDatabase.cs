using System;
using System.Collections.Generic;

namespace AnkiBot.Infrastructure
{
    public interface IDatabase<T>
    {
        void Save(T item);
        T Get(string id);
        IEnumerable<T> Where(Func<T, bool> filter);

    }
}