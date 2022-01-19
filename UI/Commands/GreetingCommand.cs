using System.Threading.Tasks;
using Domain;

namespace UI.Commands
{
    public class GreetingCommand : ICommand
    {
        private const string GreetingMessage = "Привет, я AnkiBot. Создай колоду и начни учить!";

        public string Name => "/start";
        public bool IsInitial => true;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            await bot.SendMessageWithKeyboard(user, GreetingMessage, KeyboardProvider.DefaultKeyboard);
            return ICommandInfo.Create<StartCommand>();
        }
    }
}