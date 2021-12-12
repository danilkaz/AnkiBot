using System.Threading.Tasks;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public interface ICommand
    {
        string Name { get; }
        Task<IDialog> Execute(long userId, string message, Bot bot);
    }
}