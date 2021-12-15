using System;
using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain
{
    public class Card
    {
        public Card(User user, string deckId, string front, string back, IParameters parameters)
        {
            User = user;
            DeckId = deckId;
            Front = front;
            Back = back;
            Id = Guid.NewGuid();
            TimeBeforeLearn = new TimeSpan(1, 0, 0, 0, 0);
            LastLearnTime = new DateTime(0);
            Parameters = parameters;
        }


        public Card(string id, User user, string deckId, string front, string back, TimeSpan timeBeforeLearn,
            DateTime lastLearnTime, IParameters parameters)
        {
            Id = Guid.Parse(id);
            User = user;
            DeckId = deckId;
            Front = front;
            Back = back;
            TimeBeforeLearn = timeBeforeLearn;
            LastLearnTime = lastLearnTime;
            Parameters = parameters;
        }

        public Guid Id { get; }
        public string Front { get; }
        public string Back { get; }
        public User User { get; }
        public string DeckId { get; }
        public TimeSpan TimeBeforeLearn { get; set; }
        public DateTime LastLearnTime { get; set; }
        public IParameters Parameters { get; set; }

        public DateTime NextLearnTime => LastLearnTime + TimeBeforeLearn;
    }
}