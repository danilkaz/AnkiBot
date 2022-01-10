namespace UI.Commands
{
    public interface ICommandFactory<in TData, out TCom> where TCom : ICommand
    {
        TCom CreateCommand(TData data);
    }
}