using System.Threading.Tasks;
using Domain;

namespace UI.Dialogs
{
    public interface IDialog
    {
        Task<IDialog> Execute(User user, string message, IBot bot);
    }
}