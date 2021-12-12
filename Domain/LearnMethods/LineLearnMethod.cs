using System;
using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain.LearnMethods
{
    public class LineLearnMethod : ILearnMethod
    {
        public string Name => "Линейный алгоритм запоминания";

        public TimeSpan GetNextRepetition(Card card, int answer)
        {
            var currentRepetition = card.TimeBeforeLearn;
            return currentRepetition * 2;
        }

        public IParameters GetParameters()
        {
            return new EmptyParameters();
        }
    }
}