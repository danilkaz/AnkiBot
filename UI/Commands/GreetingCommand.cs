using System.Threading.Tasks;
using AnkiBot.App;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class GreetingCommand : ICommand
    {
        public string Name => "/start";
        private readonly IRepository repository;

        public GreetingCommand(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDialog> Execute(long userId, string message, IBot bot)
        {
            var greetingMessage = "Ку я чат бот!"; //TODO: написать нормальное приветственное сообщение!
            await bot.SendMessage(userId, greetingMessage);
            return null;
        }
    }
}