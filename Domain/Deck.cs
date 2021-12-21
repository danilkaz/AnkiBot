using System;
using System.Collections.Generic;
using System.Linq;
using Domain.LearnMethods;

namespace Domain
{
    public class Deck
    {
        public Deck(User user, string name, ILearnMethod learnMethod, IEnumerable<Card> cards)
        {
            Name = name;
            User = user;
            Id = Guid.NewGuid();
            LearnMethod = learnMethod;
            Cards = cards;
        }

        public Deck(Guid id, User user, string name, ILearnMethod learnMethod, IEnumerable<Card> cards)
        {
            Id = id;
            Name = name;
            User = user;
            LearnMethod = learnMethod;
            Cards = cards;
        }

        public Guid Id { get; }
        public User User { get; }
        public string Name { get; }
        public ILearnMethod LearnMethod { get; }
        public IEnumerable<Card> Cards { get; }

        public IEnumerable<Card> GetCardsToLearn()
        {
            return Cards.Where(c => c.NextLearnTime < DateTime.Now).OrderBy(c => c.NextLearnTime);
        }
    }
}