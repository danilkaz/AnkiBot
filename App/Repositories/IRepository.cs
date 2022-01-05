using System;
using System.Collections.Generic;

namespace App
{
    public interface IRepository<T>
    {
        void Save(T obj);
        T Get(string id);
        void Update(T obj);
        void Delete(string id);
        IEnumerable<T> Search(Func<T, bool> filter);
    }
}