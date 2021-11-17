using System;
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
    public class TelegramBot
    {
        private readonly TelegramBotClient bot;
        private readonly ICommand[] commands;

        public TelegramBot(TelegramBotClient bot, ICommand[] commands)
        {
            this.bot = bot;
            this.commands = commands;
        }

        public void Start()
        {
            using var cts = new CancellationTokenSource();
            bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), null, cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _                                       => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var message = update.Message;

            foreach (var command in commands)
            {
                if (command.Name.Equals(message.Text))
                {
                    await command.Execute(message, bot);
                }
            }
        }
    }
}