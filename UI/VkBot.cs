using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Config;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Keyboard;
using User = Domain.User;

namespace UI
{
    public class VkBot : IBot
    {
        private readonly VkApi api;
        private readonly BotHandler botHandler;
        private readonly VkConfig config;

        public VkBot(VkConfig config, BotHandler botHandler)
        {
            this.config = config;
            this.botHandler = botHandler;
            api = new();
        }

        public async void Start()
        {
            await api.AuthorizeAsync(new ApiAuthParams { AccessToken = config.Token });
            while (true)
            {
                var s = await api.Groups.GetLongPollServerAsync(ulong.Parse(config.GroupId));
                var poll = await api.Groups.GetBotsLongPollHistoryAsync(
                    new() { Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 25 });
                if (poll?.Updates == null)
                    continue;
                foreach (var update in poll.Updates.Where(u => u.Type == GroupUpdateType.MessageNew))
                {
                    var userMessage = update.Message.Text;
                    var userId = update.Message.FromId;
                    if (userId is null)
                        continue;

                    await api.Messages.MarkAsReadAsync(userId.Value.ToString());
                    if (userMessage == "")
                        continue;
                    var user = new User(userId.Value.ToString());
                    await botHandler.HandleTextMessage(user, userMessage, SendMessageWithKeyboard, this);
                }
            }
        }

        public async Task SendMessage(User user, string text, bool clearKeyboard = true)
        {
            var keyboard = new KeyboardBuilder().Build();
            if (!clearKeyboard)
                keyboard = null;
            await api.Messages.SendAsync(new()
            {
                PeerId = long.Parse(user.Id),
                Message = text,
                Keyboard = keyboard,
                RandomId = new Random().Next()
            });
        }

        public async Task SendMessageWithKeyboard(User user, string text,
            KeyboardProvider keyboardProvider)
        {
            var keyboard = MakeKeyboard(keyboardProvider.Keyboard);
            await api.Messages.SendAsync(new()
            {
                PeerId = long.Parse(user.Id),
                Message = text,
                Keyboard = keyboard,
                RandomId = new Random().Next()
            });
        }

        private static MessageKeyboard MakeKeyboard(IEnumerable<IEnumerable<string>> labels)
        {
            var buttons = labels.Select(
                b => b.Select(label =>
                    new MessageKeyboardButton
                    {
                        Action = new()
                        {
                            Type = KeyboardButtonActionType.Text,
                            Label = label
                        },
                        Color = KeyboardButtonColor.Primary
                    }));
            return new() { Buttons = buttons };
        }
    }
}