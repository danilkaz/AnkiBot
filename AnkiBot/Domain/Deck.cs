using System.Collections;
using System.Collections.Generic;

namespace AnkiBot.Domain
{
    public record Deck : IEnumerable<Card>
    {
        private readonly List<Card> cards;

        public Deck(string name)
        {
            Name = name;
            cards = new List<Card>();
        }

        public string Name { get; }
        public int Count => cards.Count;

        public Card this[int index] => cards[index];

        public IEnumerator<Card> GetEnumerator()
        {
            foreach (var card in cards)
                yield return card;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }
    }
}