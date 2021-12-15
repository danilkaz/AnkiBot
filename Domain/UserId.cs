namespace AnkiBot.Domain
{
    public record User
    {
        public User(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}