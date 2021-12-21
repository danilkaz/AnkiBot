namespace App.UIClasses
{
    public class UICard
    {
        public UICard(string id, string front, string back)
        {
            Id = id;
            Front = front;
            Back = back;
        }

        public string Id { get; }
        public string Front { get; }
        public string Back { get; }
    }
}