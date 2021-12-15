using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract Task<IDialog> Execute(User user, string message, Bot bot);
    }
}