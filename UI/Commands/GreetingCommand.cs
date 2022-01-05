using System.Threading.Tasks;
using Domain;

namespace UI.Commands
{
    public class GreetingCommand : Command
    {
        public override string Name => "/start";
        public override bool isInitial => true;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            var greetingMessage = "Привет, я anki бот. Создай колоду и начни учить!";
            await bot.SendMessage(user, greetingMessage);
            return context;
        }
    }
}