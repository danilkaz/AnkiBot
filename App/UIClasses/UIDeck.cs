namespace App.UIClasses
{
    public class UIDeck
    {
        public UIDeck(string id, string name, string learnMethod)
        {
            Id = id;
            Name = name;
            LearnMethod = learnMethod;
        }

        public string Id { get; }
        public string Name { get; }
        public string LearnMethod { get; }
    }
}