using System;
using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain.LearnMethods
{
    public interface ILearnMethod
    {
        string Name { get; }

        string Description { get; }

        TimeSpan GetNextRepetition(Card card, int answer);

        IParameters GetParameters();
    }
}