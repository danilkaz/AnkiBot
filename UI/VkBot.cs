using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiBot.UI.Commands;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace UI
{
    public class VKBot : IBot
    {
        private readonly VkApi api;

        public VKBot(VkApi api)
        {
            this.api = api;
        }
        
        public void Start()
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage(long chatId, string text, bool clearKeyboard = true)
        {
            var keyboard = new KeyboardBuilder().Build();
            if (!clearKeyboard)
                keyboard = null;
            await api.Messages.SendAsync(new MessagesSendParams
            {
                PeerId = chatId,
                Message = text,
                Keyboard = keyboard,
                RandomId = new Random().Next()
            });
        }

        public async Task SendMessageWithKeyboard(long chatId, string text, IEnumerable<IEnumerable<string>> labels = null)
        {
            var keyboard = MakeKeyboard(labels);
            await api.Messages.SendAsync(new MessagesSendParams
            {
                PeerId = chatId,
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