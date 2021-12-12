using System;
using AnkiBot.Domain.LearnMethods;

namespace AnkiBot.Domain
{
    public sealed class Deck
    {
        public Deck(string userId, string name, ILearnMethod learnMethod)
        {
            Name = name;
            UserId = userId;
            Id = Guid.NewGuid();
            LearnMethod = learnMethod;
        }

        public Guid Id { get; }
        public string UserId { get; }
        public string Name { get; }
        public ILearnMethod LearnMethod { get; }
    }
}