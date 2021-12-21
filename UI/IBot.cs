using System.Threading.Tasks;
using Domain;

namespace UI
{
    public interface IBot
    {
        void Start();
        Task SendMessage(User user, string text, bool clearKeyboard = true);
        Task SendMessageWithKeyboard(User user, string text, KeyboardProvider keyboardProvider);
    }
}