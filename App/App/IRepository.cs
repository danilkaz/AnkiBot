using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AnkiBot.Domain;

namespace AnkiBot.App
{
    public interface IRepository<T>
    {
        void SaveCard(Card card);
        Card GetCard(string cardId);
        void SaveDeck(Deck deck);
        Deck GetDeck(string deckId);
        IEnumerable<Deck> GetDecksByUserId(string userId);
        IEnumerable<Card> GetCardsByDeckId(string deckId);
        IEnumerable<Card> GetCardsToLearn(string deckId);
    }
}
