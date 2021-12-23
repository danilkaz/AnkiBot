using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UI.Config;
using User = Domain.User;

namespace UI
{
    public class TelegramBot : IBot
    {
        private readonly TelegramBotClient bot;
        private readonly BotHandler botHandler;

        public TelegramBot(TelegramConfig config, BotHandler botHandler)
        {
            this.botHandler = botHandler;
            bot = new TelegramBotClient(config.Token);
        }

        public void Start()
        {
            bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync));
        }

        public async Task SendMessage(User user, string text, bool clearKeyboard = true)
        {
            ReplyMarkupBase reply = new ReplyKeyboardRemove();
            if (!clearKeyboard)
                reply = null;
            await bot.SendTextMessageAsync(user.Id, text, replyMarkup: reply);
        }

        public async Task SendMessageWithKeyboard(User user, string text,
            KeyboardProvider keyboardProvider)
        {
            var keyboard =
                keyboardProvider.Keyboard.Select(x => x.Select(y => new KeyboardButton(y)));
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
            await botHandler.HandleTextMessage(user, update.Message.Text, SendMessageWithKeyboard, this);
        }
    }
}