using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnkiBot.UI.Commands;
using VkNet;
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

        public Task SendMessage(long chatId, string text, bool clearKeyboard = true)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageWithKeyboard(long chatId, string text, IEnumerable<IEnumerable<string>> buttons = null)
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage(long chatId, string text, IEnumerable<IEnumerable<string>> buttons = null)
        {
            await api.Messages.SendAsync(new MessagesSendParams
            {
                PeerId = chatId,
                Message = text,
                RandomId = new Random().Next()
            });
        }
    }
}