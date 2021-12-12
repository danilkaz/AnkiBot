using System.Threading.Tasks;
using AnkiBot.UI.Commands;

namespace UI.Dialogs
{
    public interface IDialog
    {
        Task<IDialog> Execute(long userId, string message, Bot bot);
    }
}