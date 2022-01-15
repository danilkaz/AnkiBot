using System;
using System.Threading.Tasks;
using Domain;

namespace UI.Commands
{
    public class StartCommand : ICommand
    {
        private readonly Lazy<ICommand[]> commands;

        public StartCommand(Lazy<ICommand[]> commands)
        {
            this.commands = commands;
        }

        public string Name => "";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot) //это GreetingCommand
        {
            foreach (var command in commands.Value)
            {
                if (!command.Name.Equals(message)) continue;
                var nextCommandInfo = await command.Execute(user, message, bot);
                return nextCommandInfo;
            }

            await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
            return ICommandInfo.Create<StartCommand>();
            ;
        }
    }
}