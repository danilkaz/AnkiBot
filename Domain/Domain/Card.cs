using System;

namespace AnkiBot.Domain
{
    public class Card
    {
        public Card(string userId, string deckId, string front, string back)
        {
            UserId = userId;
            DeckId = deckId;
            Front = front;
            Back = back;
            Id = Guid.NewGuid();
            DaysBeforeLearn = new DateTime(0, 0, 1);
            LastLearnTime = new DateTime(0);
        }

        public Guid Id { get; }
        public string Front { get; }
        public string Back { get; }
        public string UserId { get; }
        public string DeckId { get; }
        public DateTime DaysBeforeLearn { get; }
        public DateTime LastLearnTime { get; }
    }
}