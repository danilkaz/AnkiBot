namespace AnkiBot.Domain.Parameters
{
    public record SuperMemo2Parameters : IParameters
    {
        public SuperMemo2Parameters(double ef = 2.5)
        {
            EF = ef;
        }

        public double EF { get; set; }
    }
}