using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiBot.Domain
{
    public record Card
    {
        public string Front { get; }
        public string Back { get; }
        
        public Card(string front, string back)
        {
            Front = front;
            Back = back;
        }
    }
}
