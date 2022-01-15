using System;
using System.Threading.Tasks;
using Domain;
using Ninject;
using UI.Commands;

namespace UI
{
    public class BotHandler
    {
        private readonly ContextApi contextApi;
        private readonly GreetingCommand greetingCommand;
        private readonly StandardKernel standardKernel;

        public BotHandler(ContextApi contextApi, GreetingCommand greetingCommand, StandardKernel standardKernel)
        {
            this.contextApi = contextApi;
            this.greetingCommand = greetingCommand;
            this.standardKernel = standardKernel;
        }

        public async Task HandleTextMessage(User user, string message,
            Func<User, string, KeyboardProvider, Task> sendMessageWithKeyboard,
            IBot bot)
        {
            try
            {
                var command = contextApi.Get(user)?.GetCommand(standardKernel) ?? greetingCommand;
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