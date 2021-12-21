using System.Threading.Tasks;
using Domain;
using UI.Dialogs;

namespace UI.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract Task<IDialog> Execute(User user, string message, IBot bot);
    }
}