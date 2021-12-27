using System.Collections.Generic;
using App.SerializedClasses;
using Domain;

namespace App
{
    public interface IRepository
    {
        void SaveCard(DbCard card);
        DbCard GetCard(string cardId);
        void UpdateCard(DbCard card);
        void DeleteCard(string cardId);
        void SaveDeck(DbDeck deck);
        DbDeck GetDeck(string deckId);
        void DeleteDeck(string deckId);
        IEnumerable<DbDeck> GetDecksByUser(User user);
        IEnumerable<DbCard> GetCardsByDeckId(string deckId);
    }
}