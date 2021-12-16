using System.Collections.Generic;
using AnkiBot.Domain;

namespace AnkiBot.App
{
    public interface IRepository
    {
        void SaveCard(Card card);
        Card GetCard(string cardId);
        void UpdateCard(Card card);
        void DeleteCard(string cardId);
        void SaveDeck(Deck deck);
        Deck GetDeck(string deckId);
        void DeleteDeck(string deckId);
        IEnumerable<string> GetDecksNamesByUser(User user);
        IEnumerable<Deck> GetDecksByUser(User user);
    }
}