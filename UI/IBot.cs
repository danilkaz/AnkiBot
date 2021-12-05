using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnkiBot.UI.Commands
{
    public interface IBot
    {
        void Start();
        Task SendMessage(long chatId, string text, bool clearKeyboard = true);
        Task SendMessageWithKeyboard(long chatId, string text, IEnumerable<IEnumerable<string>> buttons = null);
    }
}