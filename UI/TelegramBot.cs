using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnkiBot.UI.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;


namespace AnkiBot.UI
{
    public class TelegramBot : IBot
    {
        private readonly TelegramBotClient bot;
        private readonly ICommand[] commands;
        private readonly Dictionary<long, ICommand> usersCommands;

        public TelegramBot(TelegramBotClient bot, ICommand[] commands)
        {
            this.bot = bot;
            this.commands = commands;
            usersCommands = new Dictionary<long, ICommand>();
        }

        public void Start()
        {
            using var cts = new CancellationTokenSource();
            bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), null, cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }

        public async Task SendMessage(long chatId, string text)
        {
            await bot.SendTextMessageAsync(chatId, text);
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

            if (!usersCommands.ContainsKey(userId) || usersCommands[userId] == null)
                foreach (var command in commands)
                {
                    if (command.Name.Equals(message.Text))
                    {
                        usersCommands[userId] = await command.Execute(userId, message.Text, this);
                    }
                }
            else
                usersCommands[userId] = await usersCommands[userId].Execute(userId, message.Text, this);
        }
    }
}