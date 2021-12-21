using AnkiBot.Domain.Parameters;
using AnkiBot.Domain.Visitors;

namespace AnkiBot.Domain.LearnMethods
{
    public class LineLearnMethod : ILearnMethod
    {
        public string Name => "Линейный алгоритм запоминания";

        public string Description => "Линейный алгоритм запоминания\n" +
                                     "Самый простой и глупый способ для вычисления интервалов запоминания:\n" +
                                     "при изучении карточки интервал повторения увеличивается в два раза";

        public void LearnCard(Card card, int answer)
        {
            var learnVisitor = new LearnVisitor(card, answer);
            card.Parameters.Accept(learnVisitor);
        }

        public IParameters GetParameters()
        {
            return new EmptyParameters();
        }
    }
}