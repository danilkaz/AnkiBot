using System;
using System.Collections.Generic;
using System.Linq;
using AnkiBot.Domain;

namespace AnkiBot.App
{
    public class DictRepository : IRepository
    {
        private readonly Dictionary<Deck, List<Card>> decks = new();

        public void SaveCard(Card card)
        {
            var deck = GetDeck(card.DeckId);
            var index = decks[deck].FindIndex(c => c == card);
            if (index == -1) decks[deck].Add(card);
            else decks[deck][index] = card;
        }

        public Card GetCard(string cardId)
        {
            return decks.Values.SelectMany(list => list.Where(card => card.Id.ToString() == cardId)).FirstOrDefault();
        }

        public void UpdateCard(Card card)
        {
            throw new NotImplementedException();
        }

        public void DeleteCard(string cardId)
        {
            throw new NotImplementedException();
        }

        public void SaveDeck(Deck deck)
        {
            decks[deck] = new List<Card>();
        }

        public Deck GetDeck(string deckId)
        {
            return decks.Keys.FirstOrDefault(deck => deck.Id.ToString() == deckId);
        }

        public void DeleteDeck(string deckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Deck> GetDecksByUserId(string userId)
        {
            return decks.Keys.Where(deck => deck.UserId == userId);
        }

        public IEnumerable<Card> GetCardsByDeckId(string deckId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Card> GetCardsToLearn(string deckId)
        {
            return decks[GetDeck(deckId)].Where(c => c.LastLearnTime + c.TimeBeforeLearn < DateTime.Now)
                .OrderBy(c => c.LastLearnTime + c.TimeBeforeLearn);
        }
    }
}