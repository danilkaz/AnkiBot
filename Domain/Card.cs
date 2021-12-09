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
            TimeBeforeLearn = new TimeSpan(1, 0, 0, 0, 0);
            LastLearnTime = new DateTime(0);
        }

        public Guid Id { get; }
        public string Front { get; }
        public string Back { get; }
        public string UserId { get; }
        public string DeckId { get; }
        public TimeSpan TimeBeforeLearn { get; set; }
        public DateTime LastLearnTime { get; set; }
        public DateTime NextLearnTime => LastLearnTime + TimeBeforeLearn;
    }
}