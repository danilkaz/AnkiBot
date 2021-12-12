using System;
using System.Collections.Generic;
using System.Linq;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using Infrastructure.Attributes;

namespace App.SerializedClasses
{
    [Table("decks")]
    public class DbDeck
    {
        [Field("id", isUnique: true)]
        public string Id { get; }
        
        [Field("userId")]
        public string UserId { get; }
        
        [Field("name")]
        public string Name { get; }
        
        [Field("learnMethod")]
        public string LearnMethod { get; }
        
        
        [Constructor]
        public DbDeck(string id, string userId, string name, string learnMethod)
        {
            Id = id;
            UserId = userId;
            Name = name;
            LearnMethod = learnMethod;
        }
        
        public DbDeck(Deck deck)
        {
            Id = deck.Id.ToString();
            UserId = deck.UserId;
            Name = deck.Name;
            LearnMethod = deck.LearnMethod.Name;
        }
    }
}