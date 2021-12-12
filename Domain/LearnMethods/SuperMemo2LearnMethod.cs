using System;
using AnkiBot.Domain.Parameters;

namespace AnkiBot.Domain.LearnMethods
{
    public class SuperMemo2LearnMethod : ILearnMethod
    {
        public string Name => "Алгоритм SuperMemo 2";

        public TimeSpan GetNextRepetition(Card card, int answer)
        {
            throw new NotImplementedException();
        }

        public IParameters GetParameters()
        {
            return new SuperMemo2Parameters();
        }
    }
}