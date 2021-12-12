namespace AnkiBot.Domain.Parameters
{
    public record SuperMemo2Parameters : IParameters
    {
        public SuperMemo2Parameters()
        {
            EF = 2.5;
        }

        public double EF { get; }
    }
}