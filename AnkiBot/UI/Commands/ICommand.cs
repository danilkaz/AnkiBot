namespace AnkiBot.UI.Commands
{
    internal interface ICommand
    {
        string Name { get; set; }

        public void Execute(string message);
    }
}