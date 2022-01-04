using System;
using System.Collections;
using System.Collections.Generic;
using App.SerializedClasses;
using Domain;

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