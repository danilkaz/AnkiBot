using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Domain;
using Ninject;
using UI.Commands;

namespace UI
{
    public class BotHandler
    {
        private readonly Dictionary<string, ICommand> commands;
        private readonly ContextApi contextApi;
        private readonly GreetingCommand greetingCommand;
        private readonly StandardKernel standardKernel;
        private readonly StartCommand startCommand;

        public BotHandler(ContextApi contextApi, IEnumerable<ICommand> commands, GreetingCommand greetingCommand,
            StartCommand startCommand, StandardKernel standardKernel)
        {
            this.contextApi = contextApi;
            this.greetingCommand = greetingCommand;
            this.startCommand = startCommand;
            this.standardKernel = standardKernel;
            this.commands = commands.ToDictionary(c => c.Name);
        }

        public async Task HandleTextMessage(User user, string message,
            Func<User, string, KeyboardProvider, Task> sendMessageWithKeyboard,
            IBot bot)
        {
            try
            {
                var command =
                    contextApi.Get(user)?.GetCommand(standardKernel) ?? greetingCommand;
                var nextCommand = await command.Execute(user, message, bot);
                contextApi.Update(user, nextCommand);
            }
            catch (Exception e)
            {
                await sendMessageWithKeyboard(user, "Ой! Что-то пошло не так :(", KeyboardProvider.DefaultKeyboard);
                Console.WriteLine(e);
                contextApi.Update(user, ICommandInfo.Create<StartCommand>());
            }
        }
    }
}