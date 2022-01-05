using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App;
using Domain;
using UI.Commands;

namespace UI
{
    public class BotHandler
    {
        private readonly Dictionary<string, Command> commands;
        private readonly ContextApi contextApi;

        public BotHandler(ContextApi contextApi, Command[] commands)
        {
            this.contextApi = contextApi;
            this.commands = new Dictionary<string, Command>();
            foreach (var command in commands)
                this.commands[command.Name] = command;
        }

        public async Task HandleTextMessage(User user, string message,
            Func<User, string, KeyboardProvider, Task> sendMessageWithKeyboard, IBot bot)
        {
            try
            {
                var context = contextApi.Get(user);
                var nextContext = new Context();
                if (context?.CommandName is null && commands.ContainsKey(message) && commands[message].isInitial)
                    nextContext = await commands[message].Execute(user, message, bot, new Context());
                else if (context?.CommandName is not null && commands.ContainsKey(context.CommandName))
                    nextContext = await commands[context.CommandName].Execute(user, message, bot, context);

                if (nextContext.CommandName is null)
                    await sendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                contextApi.Update(user, nextContext);
            }
            catch (Exception e)
            {
                await sendMessageWithKeyboard(user, "Ой! Что-то пошло не так :(", KeyboardProvider.DefaultKeyboard);
                Console.WriteLine(e);
                contextApi.Update(user, new Context());
            }
        }
    }
}