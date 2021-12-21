using System;
using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain.LearnMethods
{
    public interface ILearnMethod
    {
        string Name { get; }

        string Description { get; }

        void LearnCard(Card card, int answer);

        IParameters GetParameters();
    }
}