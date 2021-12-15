using System;
using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain.LearnMethods
{
    public class LineLearnMethod : ILearnMethod
    {
        public string Name => "Линейный алгоритм запоминания";

        public string Description => "Линейный алгоритм запоминания\n" +
                                     "Самый простой и глупый способ для вычисления интервалов запоминания:\n" +
                                     "при изучении карточки интервал повторения увеличивается в два раза";

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