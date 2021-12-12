using System;
using System.Collections;
using System.Collections.Generic;
using AnkiBot.Domain.LearnMethods;


namespace AnkiBot.Domain
{
    public class Deck
    {
        public Deck(string userId, string name, ILearnMethod learnMethod)
        {
            Name = name;
            UserId = userId;
            Id = Guid.NewGuid();
            LearnMethod = learnMethod;
        }
        
        public Deck(string id, string userId, string name, ILearnMethod learnMethod)
        {
            Id = Guid.Parse(id);
            Name = name;
            UserId = userId;
            LearnMethod = learnMethod;
        }

        public Guid Id { get; }
        public string UserId { get; }
        public string Name { get; }
        public ILearnMethod LearnMethod { get; }
    }
}