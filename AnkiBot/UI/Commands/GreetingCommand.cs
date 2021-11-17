using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnkiBot.UI.Commands
{
    public class GreetingCommand : ICommand
    {
        public string Name => "/start";
        public async Task Execute(Message message, TelegramBotClient bot)
        {
            var chatId = message.Chat.Id;

            var greetingMessage = "Ку я чат бот!"; //TODO: написать нормальное приветственное сообщение!
           
            await bot.SendTextMessageAsync(chatId, greetingMessage);
        }
    }
}