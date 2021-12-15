using System;
using AnkiBot.Domain.LearnMethods;

namespace AnkiBot.Domain
{
    public class Deck
    {
        public Deck(User user, string name, ILearnMethod learnMethod)
        {
            Name = name;
            User = user;
            Id = Guid.NewGuid();
            LearnMethod = learnMethod;
        }

        public Deck(string id, User user, string name, ILearnMethod learnMethod)
        {
            Id = Guid.Parse(id);
            Name = name;
            User = user;
            LearnMethod = learnMethod;
        }

        public Guid Id { get; }
        public User User { get; }
        public string Name { get; }
        public ILearnMethod LearnMethod { get; }
    }
}