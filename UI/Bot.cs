using System.Collections.Generic;
using System.Threading.Tasks;
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

        private readonly Dictionary<long, IDialog> usersStates = new();

        protected Bot(Command[] commands)
        {
            this.commands = commands;
        }

        public abstract void Start();
        public abstract Task SendMessage(long chatId, string text, bool clearKeyboard = true);
        public abstract Task SendMessageWithKeyboard(long chatId, string text, IEnumerable<IEnumerable<string>> labels);

        public async Task HandleTextMessage(long userId, string message)
        {
            if (usersStates.ContainsKey(userId) && usersStates[userId] != null)
                usersStates[userId] = await usersStates[userId].Execute(userId, message, this);
            else
                foreach (var command in commands)
                    if (command.Name.Equals(message))
                        usersStates[userId] = await command.Execute(userId, message, this);
            if (usersStates.ContainsKey(userId) && usersStates[userId] is null)
                await SendMessageWithKeyboard(userId, "Выберите команду:", defaultKeyboard);
        }
    }
}