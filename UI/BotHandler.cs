using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using UI.Commands;
using UI.Dialogs;

namespace UI
{
    public class BotHandler
    {
        private readonly Command[] commands;

        private readonly Dictionary<User, IDialog> usersStates = new();

        public BotHandler(Command[] commands)
        {
            this.commands = commands;
        }

        public async Task HandleTextMessage(User user, string message,
            Func<User, string, KeyboardProvider, Task> sendMessageWithKeyboard, IBot bot)
        {
            {
                try
                {
                    if (usersStates.ContainsKey(user) && usersStates[user] != null)
                        usersStates[user] = await usersStates[user].Execute(user, message, bot);
                    else
                        foreach (var command in commands)
                            if (command.Name.Equals(message))
                                usersStates[user] = await command.Execute(user, message, bot);
                    if (usersStates.ContainsKey(user) && usersStates[user] is null)
                        await sendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                }
                catch
                {
                    await sendMessageWithKeyboard(user, "Ой! Что-то пошло не так :(", KeyboardProvider.DefaultKeyboard);
                    usersStates[user] = null;
                }
            }
        }
    }
}