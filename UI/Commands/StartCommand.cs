using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using JetBrains.Annotations;
using UI.Data;

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

        public async Task<INext> Execute(User user, string message, IBot bot) //это GreetingCommand
        {
            foreach (var command in commands.Value)
            {
                if (!command.Name.Equals(message)) continue;
                var nextCommand = await command.Execute(user, message, bot);
                // if (nextCommand.Command.Name == Name) //TODO: починить
                //TODO: сделать обработку неправильной команды
                await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                return nextCommand;
            }
            
            return INext.Create<StartCommand>();
        }
    }
}