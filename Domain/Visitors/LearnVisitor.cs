using System;
using Domain.Parameters;

namespace Domain.Visitors
{
    public class LearnVisitor : IVisitor
    {
        private readonly int answer;
        private readonly Card card;

        public LearnVisitor(Card card, int answer)
        {
            this.card = card;
            this.answer = answer;
        }

        public void VisitEmptyParameters(EmptyParameters parameters)
        {
            card.TimeBeforeLearn *= 2;
        }

        public void VisitSuperMemo2Parameters(SuperMemo2Parameters parameters)
        {
            if (answer <= 3)
            {
                card.TimeBeforeLearn = new TimeSpan(1, 0, 0, 0);
                return;
            }

            card.TimeBeforeLearn *= parameters.EF;

            parameters.EF += 0.1 - (5 - answer) * (0.08 + (5 - answer) * 0.02);
            if (parameters.EF < 1.3)
                parameters.EF = 1.3;
        }
    }
}