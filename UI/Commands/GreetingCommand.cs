using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnkiBot.UI.Commands
{
    public class GreetingCommand : ICommand
    {
        public string Name => "/start";
        public async Task<ICommand> Execute(long userId, string message, IBot bot)
        {
            var greetingMessage = "Ку я чат бот!"; //TODO: написать нормальное приветственное сообщение!
            await bot.SendMessage(userId, greetingMessage);
            return null;
        }
    }
}