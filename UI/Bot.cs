using System.Collections.Generic;
using System.Threading.Tasks;
using AnkiBot.Domain;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public abstract class Bot
    {
        private readonly Command[] commands;

        private readonly string[][] defaultKeyboard =
        {
            new[]
            {
                "Создать колоду", "Удалить колоду"
            },
            new[]
            {
                "Добавить карточку", "Удалить карточку"
            },
            new[]
            {
                "Учить колоду"
            }
        };

        private readonly Dictionary<User, IDialog> usersStates = new();

        protected Bot(Command[] commands)
        {
            this.commands = commands;
        }

        public abstract void Start();
        public abstract Task SendMessage(User user, string text, bool clearKeyboard = true);
        public abstract Task SendMessageWithKeyboard(User user, string text, IEnumerable<IEnumerable<string>> labels);

        protected async Task HandleTextMessage(User user, string message)
        {
            if (usersStates.ContainsKey(user) && usersStates[user] != null)
                usersStates[user] = await usersStates[user].Execute(user, message, this);
            else
                foreach (var command in commands)
                    if (command.Name.Equals(message))
                        usersStates[user] = await command.Execute(user, message, this);
            if (usersStates.ContainsKey(user) && usersStates[user] is null)
                await SendMessageWithKeyboard(user, "Вот что я умею:", defaultKeyboard);
        }
    }
}