using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.UI.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UI.Dialogs;


namespace AnkiBot.UI
{
    public class TelegramBot : IBot
    {
        private readonly TelegramBotClient bot;
        private readonly ICommand[] commands;
        private readonly Dictionary<long, IDialog> usersStates;
        private readonly IRepository repository;

        private string[][] defaultKeyboard;

        public TelegramBot(TelegramBotClient bot, ICommand[] commands, IRepository repository)
        {
            this.bot = bot;
            this.commands = commands;
            this.repository = repository;
            usersStates = new Dictionary<long, IDialog>();

            defaultKeyboard = new[]
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
        }

        public void Start()
        {
            using var cts = new CancellationTokenSource();
            bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), null, cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }

        public async Task SendMessage(long chatId, string text, bool clearKeyboard = false)
        {
            ReplyMarkupBase reply = new ReplyKeyboardRemove();
            if (!clearKeyboard)
                reply = null;
            await bot.SendTextMessageAsync(chatId, text, replyMarkup: reply);
        }

        public async Task SendMessageWithKeyboard(long chatId, string text, IEnumerable<IEnumerable<string>> buttons)
        {
            var keyboard =
                buttons.Select(x => x.Select(y => new KeyboardButton(y)));
            await bot.SendTextMessageAsync(chatId, text, replyMarkup: new ReplyKeyboardMarkup(keyboard));
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var message = update.Message;
            var userId = message.Chat.Id;

            if (usersStates.ContainsKey(userId) && usersStates[userId] != null)
                usersStates[userId] = await usersStates[userId].Execute(userId, message.Text, this);
            else
                foreach (var command in commands)
                    if (command.Name.Equals(message.Text))
                        usersStates[userId] = await command.Execute(userId, message.Text, this);
            if (usersStates.ContainsKey(userId) && usersStates[userId] is null)
                await SendMessageWithKeyboard(userId, "Выберите команду:", defaultKeyboard);
        }
    }
}