using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnkiBot.Domain;
using UI;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class Bot
    {
        private readonly Command[] commands;

        private readonly Dictionary<User, IDialog> usersStates = new();

        public Bot(Command[] commands)
        {
            this.commands = commands;
        }
        
        public async Task HandleTextMessage(User user, string message, Func<User, string, KeyboardProvider, Task> SendMessageWithKeyboard, IBot bot)
        {
            if (usersStates.ContainsKey(user) && usersStates[user] != null)
                usersStates[user] = await usersStates[user].Execute(user, message, bot);
            else
                foreach (var command in commands)
                    if (command.Name.Equals(message))
                        usersStates[user] = await command.Execute(user, message, bot);
            if (usersStates.ContainsKey(user) && usersStates[user] is null)
                await SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
        }
    }
}