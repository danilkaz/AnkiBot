using System.Threading.Tasks;
using App;
using Domain;
using UI.Dialogs;

namespace UI.Commands
{
    public class GreetingCommand : Command
    {
        private readonly IRepository repository;

        public GreetingCommand(IRepository repository)
        {
            this.repository = repository;
        }

        public override string Name => "/start";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var greetingMessage = "Привет, я anki бот. Создай колоду и начни учить!";
            await bot.SendMessage(user, greetingMessage);
            return null;
        }
    }
}