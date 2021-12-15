using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiBot.UI.Commands;
using UI.Config;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
using User = AnkiBot.Domain.User;

namespace UI
{
    public class VkBot : Bot
    {
        private readonly VkConfig config;
        private readonly VkApi api;

        public VkBot(VkConfig config, Command[] commands) : base(commands)
        {
            this.config = config;
            api = new VkApi();
        }
        
        public override async void Start()
        {
            await api.AuthorizeAsync(new ApiAuthParams {AccessToken = config.Token});
            while (true)
            {
                var s = await api.Groups.GetLongPollServerAsync(ulong.Parse(config.GroupId));
                var poll = await api.Groups.GetBotsLongPollHistoryAsync(
                    new BotsLongPollHistoryParams
                        {Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 25});
                if (poll?.Updates == null)
                    continue;
                foreach (var update in poll.Updates.Where(u => u.Type == GroupUpdateType.MessageNew))
                {
                    var userMessage = update.Message.Text;
                    var userId = update.Message.FromId;
                    
                    await api.Messages.MarkAsReadAsync(userId.Value.ToString());
                    if (userMessage == "")
                        continue;
                    var user = new User(userId.Value.ToString());
                    await HandleTextMessage(user, userMessage);
                }
            }
        }

        public override async Task SendMessage(User user, string text, bool clearKeyboard = true)
        {
            var keyboard = new KeyboardBuilder().Build();
            if (!clearKeyboard)
                keyboard = null;
            await api.Messages.SendAsync(new MessagesSendParams
            {
                PeerId = long.Parse(user.Id),
                Message = text,
                Keyboard = keyboard,
                RandomId = new Random().Next()
            });
        }

        public override async Task SendMessageWithKeyboard(User user, string text,
            IEnumerable<IEnumerable<string>> labels)
        {
            var keyboard = MakeKeyboard(labels);
            await api.Messages.SendAsync(new MessagesSendParams
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
                        Action = new MessageKeyboardButtonAction
                        {
                            Type = KeyboardButtonActionType.Text,
                            Label = label
                        },
                        Color = KeyboardButtonColor.Primary
                    }));
            return new MessageKeyboard {Buttons = buttons};
        }
    }
}