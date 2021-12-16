using System.Threading.Tasks;
using AnkiBot.Domain;
using AnkiBot.UI.Commands;

namespace UI.Dialogs
{
    public interface IDialog
    {
        Task<IDialog> Execute(User user, string message, IBot bot);
    }
}