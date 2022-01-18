namespace Domain.Parameters
{
    public record EmptyParameters : IParameters
    {
        public void LearnCard(Card card, int answer)
        {
            card.TimeBeforeLearn *= 2;
        }
    }
}