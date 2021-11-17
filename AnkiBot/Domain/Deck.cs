using System;
using System.Collections;
using System.Collections.Generic;

namespace AnkiBot.Domain
{
    public record Deck : IEnumerable<Card>
    {
        public string Name { get; }
        public int Count => cards.Count;
        private List<Card> cards;
        
        public Deck(string name)
        {
            Name = name;
            cards = new List<Card>();
        }

        public void AddCard(Card card) => cards.Add(card);

        public Card this[int index] => cards[index];

        public IEnumerator<Card> GetEnumerator()
        {
            foreach (var card in cards)
                yield return card;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}