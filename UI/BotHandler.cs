using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;
using Ninject;
using UI.Commands;
using UI.Data;
using VkNet.Enums.SafetyEnums;

namespace UI
{
    public class BotHandler
    {
        private readonly Dictionary<string, ICommand> commands;
        private readonly ContextApi contextApi;
        private readonly GreetingCommand greetingCommand;
        private readonly StartCommand startCommand;
        private readonly StandardKernel standardKernel;

        public BotHandler(ContextApi contextApi, IEnumerable<ICommand> commands, GreetingCommand greetingCommand, StartCommand startCommand, StandardKernel standardKernel)
        {
            this.contextApi = contextApi;
            this.greetingCommand = greetingCommand;
            this.startCommand = startCommand;
            this.standardKernel = standardKernel;
            this.commands = commands.ToDictionary(c => c.Name);
        }

        public async Task HandleTextMessage(User user, string message,
            Func<User, string, KeyboardProvider, Task> sendMessageWithKeyboard,
            IBot bot) //TODO: Ibot передать через зависимости
        {
            try
            {
                var command = contextApi.Get(user)?.GetCommand(standardKernel) ?? greetingCommand; //TODO: GreetingCommand
                var nextCommand = await command.Execute(user, message, bot);
                contextApi.Update(user, nextCommand);

                // var context = contextApi.Get(user);
                // var nextContext = new Context();
                // if (context?.CommandName is null && commands.ContainsKey(message) && commands[message].IsInitial)
                //     nextContext = await commands[message].Execute(user, message, bot, new Context());
                // else if (context?.CommandName is not null && commands.ContainsKey(context.CommandName))
                //     nextContext = await commands[context.CommandName].Execute(user, message, bot, context);
                //
                // if (nextContext.CommandName is null)
                //     await sendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                // contextApi.Update(user, nextContext);
            }
            catch (Exception e)
            {
                await sendMessageWithKeyboard(user, "Ой! Что-то пошло не так :(", KeyboardProvider.DefaultKeyboard);
                throw;
                contextApi.Update(user, INext.Create<StartCommand>());
            }
        }
    }
}