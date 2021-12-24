namespace Domain.Parameters
{
    public class EmptyParameters : IParameters
    {
        public void LearnCard(Card card, int answer)
        {
            card.TimeBeforeLearn *= 2;
        }
    }
}