using System.Threading.Tasks;
using AnkiBot.Domain;

namespace UI.Dialogs
{
    public interface IBot
    {
        void Start();
        Task SendMessage(User user, string text, bool clearKeyboard = true);
        Task SendMessageWithKeyboard(User user, string text, KeyboardProvider keyboardProvider);
    }
}