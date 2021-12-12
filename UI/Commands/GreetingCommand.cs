using System.Threading.Tasks;
using AnkiBot.App;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class GreetingCommand : ICommand
    {
        private readonly IRepository repository;

        public GreetingCommand(IRepository repository)
        {
            this.repository = repository;
        }

        public string Name => "/start";

        public async Task<IDialog> Execute(long userId, string message, Bot bot)
        {
            var greetingMessage = "Ку я чат бот!"; //TODO: написать нормальное приветственное сообщение!
            await bot.SendMessage(userId, greetingMessage);
            return null;
        }
    }
}