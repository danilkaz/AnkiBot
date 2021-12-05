using System;

namespace AnkiBot.Domain.LearnMethods
{
    public interface ILearnMethod
    {
        string Name { get; }
        TimeSpan GetNextRepetition(Card card, int answer);
    }
}