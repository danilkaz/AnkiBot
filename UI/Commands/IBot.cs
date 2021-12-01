using System.Threading.Tasks;

namespace AnkiBot.UI.Commands
{
    public interface IBot
    {
        void Start();
        Task SendMessage(long chatId, string text);
    }
}