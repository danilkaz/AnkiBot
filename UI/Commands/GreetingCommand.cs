using System.Threading.Tasks;
using Domain;
using UI.Dialogs;

namespace UI.Commands
{
    public class GreetingCommand : Command
    {
        public override string Name => "/start";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var greetingMessage = "Привет, я anki бот. Создай колоду и начни учить!";
            await bot.SendMessage(user, greetingMessage);
            return null;
        }
    }
}