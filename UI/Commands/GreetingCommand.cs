using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class GreetingCommand : Command
    {
        private readonly IRepository repository;

        public GreetingCommand(IRepository repository)
        {
            this.repository = repository;
        }

        public override string Name => "/start";

        public override async Task<IDialog> Execute(User user, string message, Bot bot)
        {
            var greetingMessage = "Ку я чат бот!"; //TODO: написать нормальное приветственное сообщение!
            await bot.SendMessage(user, greetingMessage);
            return null;
        }

        
    }
}