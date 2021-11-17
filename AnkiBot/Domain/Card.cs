namespace AnkiBot.Domain
{
    public record Card
    {
        public Card(string front, string back)
        {
            Front = front;
            Back = back;
        }

        public string Front { get; }
        public string Back { get; }
    }
}