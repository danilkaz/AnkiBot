using System.Threading.Tasks;
using System.Windows.Input;
using Domain;
using UI.Data;

namespace UI.Commands
{
    public class GreetingCommand : ICommand
    {
        private const string GreetingMessage = "Привет, я anki бот. Создай колоду и начни учить!";
        private readonly StartCommand startCommand;

        public GreetingCommand(StartCommand startCommand)
        {
            this.startCommand = startCommand;
        }

        public string Name => "/start";
        public bool IsInitial => true;

        public async Task<INext> Execute(User user, string message, IBot bot)
        {
            await bot.SendMessage(user, GreetingMessage);
            return INext.Create<GreetingCommand>();
        }
    }
}