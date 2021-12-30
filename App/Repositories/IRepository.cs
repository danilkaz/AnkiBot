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
        // void SaveDeck(DbDeck deck);
        // DbDeck GetDeck(string deckId);
        // void DeleteDeck(string deckId);
        // IEnumerable<DbDeck> GetDecksByUser(User user);
        // IEnumerable<DbCard> GetCardsByDeckId(string deckId);
    }
}