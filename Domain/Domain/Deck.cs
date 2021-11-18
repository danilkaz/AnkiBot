using System;
using System.Collections;
using System.Collections.Generic;

namespace AnkiBot.Domain
{
    public sealed class Deck
    {
        public Deck(string userId, string name)
        {
            Name = name;
            UserId = userId;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public string UserId { get; }
        public string Name { get; }
    }
}