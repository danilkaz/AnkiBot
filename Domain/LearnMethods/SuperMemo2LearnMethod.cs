using System;
using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain.LearnMethods
{
    public class SuperMemo2LearnMethod : ILearnMethod
    {
        public string Name => "Алгоритм SuperMemo 2";

        public string Description => "Алгоритм SuperMemo 2\n" +
                                     "Один из популярных способов для вычисления интервалов запоминания:" +
                                     "следующий интервал вычисляется на основе предыдущего интервала и ответа пользователя";
        
        // TODO: избавиться от даун каста
        public TimeSpan GetNextRepetition(Card card, int answer)
        {
            var currentRepetition = card.TimeBeforeLearn;
            var EF = ((SuperMemo2Parameters) card.Parameters).EF;
            currentRepetition *= EF;
            var newEF = EF + (0.1 - (5 - answer) * (0.08 + (5 - answer) * 0.02));
            if (newEF < 1.3)
                newEF = 1.3;
            if (answer <= 3)
                return new TimeSpan(1, 0, 0, 0);
            card.Parameters = new SuperMemo2Parameters(newEF);
            return currentRepetition;
        }

        public IParameters GetParameters()
        {
            return new SuperMemo2Parameters();
        }
    }
}