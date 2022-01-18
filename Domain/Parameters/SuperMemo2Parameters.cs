using Newtonsoft.Json;

namespace Domain.Parameters
{
    public record SuperMemo2Parameters(double EF = 2.5) : IParameters
    {
        [JsonProperty] private double EF { get; set; } = EF;

        public void LearnCard(Card card, int answer)
        {
            if (answer <= 3)
            {
                card.TimeBeforeLearn = new(1, 0, 0, 0);
                return;
            }

            card.TimeBeforeLearn *= EF;

            EF += 0.1 - (5 - answer) * (0.08 + (5 - answer) * 0.02);
            if (EF < 1.3)
                EF = 1.3;
        }
    }
}