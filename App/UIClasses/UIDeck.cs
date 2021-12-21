namespace App.UIClasses
{
    public class UIDeck
    {
        public UIDeck(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}