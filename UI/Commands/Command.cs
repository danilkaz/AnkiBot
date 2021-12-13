using System.Threading.Tasks;
using AnkiBot.App;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract Task<IDialog> Execute(long userId, string message, Bot bot);
    }
}