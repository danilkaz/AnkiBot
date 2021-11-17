using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AnkiBot
{
    public static class Program
    {
        private static readonly string Token =
            Environment.GetEnvironmentVariable("BOT_TOKEN", EnvironmentVariableTarget.User);

        public static async Task Main()
        {
            Console.WriteLine(Token);

            var botClient = new TelegramBotClient(Token);

            var me = await botClient.GetMeAsync();
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            using var cts = new CancellationTokenSource();
            botClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), null, cts.Token);

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;

            Console.WriteLine($"Received a '{update.Message.Text}' message in chat {chatId}.");

            await botClient.SendTextMessageAsync(
                chatId,
                "You said:\n" + update.Message.Text
            );
        }
    }
}