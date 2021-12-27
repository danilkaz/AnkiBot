namespace App.UIClasses
{
    public class UICard
    {
        public UICard(string id, string front, string back, string deckId)
        {
            Id = id;
            Front = front;
            Back = back;
            DeckId = deckId;
        }

        public string Id { get; }
        public string Front { get; }
        public string Back { get; }
        public string DeckId { get; }
    }
}