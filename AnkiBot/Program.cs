using System;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.UI;
using AnkiBot.UI.Commands;
using Telegram.Bot;
using Ninject;
using UI;
using UI.Dialogs;
using VkNet;
using VkNet.Model;


namespace AnkiBot
{
    public static class Program
    {
        private static readonly string TelegramToken =
            Environment.GetEnvironmentVariable("TELEGRAM_TOKEN", EnvironmentVariableTarget.User);
        private static readonly string VkToken = 
            Environment.GetEnvironmentVariable("VK_TOKEN", EnvironmentVariableTarget.User);

        public static void Main()
        {
            var bot = CreateTelegramBot();
            bot.Start();
        }

        private static TelegramBot CreateTelegramBot()
        {
            var container = new StandardKernel();
            container.Bind<IBot>().To<TelegramBot>();
            container.Bind<IBot>().To<VKBot>();
            
            container.Bind<TelegramBotClient>().ToConstant(new TelegramBotClient(TelegramToken));
            
            container.Bind<IRepository>().To<DictRepository>().InSingletonScope();
            
            container.Bind<ICommand>().To<GreetingCommand>();
            container.Bind<ICommand>().To<CreateDeckCommand>();
            container.Bind<ICommand>().To<CreateCardCommand>();
            
            container.Bind<IDialog>().To<CreateDeckDialog>();
            container.Bind<IDialog>().To<CreateCardDialog>();
            
            return container.Get<TelegramBot>();
        }

        private static async Task<VkApi> CreateVkApi()
        {
            var api = new VkApi(); 
            var p = new ApiAuthParams
            {
                AccessToken = "**"
            };
            await api.AuthorizeAsync(p);
            return api;
        }
    }
}