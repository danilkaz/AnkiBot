using Domain.Parameters;
using Domain.Visitors;

namespace Domain.LearnMethods
{
    public class SuperMemo2LearnMethod : ILearnMethod
    {
        public string Name => "Алгоритм SuperMemo 2";

        public string Description => "Алгоритм SuperMemo 2\n" +
                                     "Один из популярных способов для вычисления интервалов запоминания:" +
                                     "следующий интервал вычисляется на основе предыдущего интервала и ответа пользователя";

        public void LearnCard(Card card, int answer)
        {
            var visitor = new LearnVisitor(card, answer);
            card.Parameters.Accept(visitor);
        }

        public IParameters GetParameters()
        {
            return new SuperMemo2Parameters();
        }
    }
}