using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnkiBot.UI.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UI.Config;
using User = AnkiBot.Domain.User;

namespace UI
{
    public class TelegramBot : Bot
    {
        private readonly TelegramBotClient bot;

        public TelegramBot(TelegramConfig config, Command[] commands) : base(commands)
        {
            bot = new TelegramBotClient(config.Token);
        }

        public override void Start()
        {
            using var cts = new CancellationTokenSource();
            bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), null, cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }

        public override async Task SendMessage(User user, string text, bool clearKeyboard = true)
        {
            ReplyMarkupBase reply = new ReplyKeyboardRemove();
            if (!clearKeyboard)
                reply = null;
            await bot.SendTextMessageAsync(user.Id, text, replyMarkup: reply);
        }

        public override async Task SendMessageWithKeyboard(User user, string text,
            IEnumerable<IEnumerable<string>> labels)
        {
            var keyboard =
                labels.Select(x => x.Select(y => new KeyboardButton(y)));
            await bot.SendTextMessageAsync(user.Id, text, replyMarkup: new ReplyKeyboardMarkup(keyboard));
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
            if (update.Type != UpdateType.Message || update.Message.Type != MessageType.Text)
                return;

            var user = new User(update.Message.Chat.Id.ToString());
            await HandleTextMessage(user, update.Message.Text);
        }
    }
}