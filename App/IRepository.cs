using System.Collections.Generic;
using App.SerializedClasses;
using Domain;

namespace App
{
    public interface IRepository
    {
        void SaveCard(Card card);
        DbCard GetCard(string cardId);
        void UpdateCard(Card card);
        void DeleteCard(string cardId);
        void SaveDeck(Deck deck);
        DbDeck GetDeck(string deckId);
        void DeleteDeck(string deckId);
        IEnumerable<DbDeck> GetDecksByUser(User user);
        IEnumerable<DbCard> GetCardsByDeckId(string deckId);
    }
}