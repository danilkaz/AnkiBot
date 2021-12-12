using System;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.UI;
using AnkiBot.UI.Commands;
using Ninject;
using Telegram.Bot;
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
            // var bot = CreateTelegramBot();
            // bot.Start();
            var container = CreateContainer();
            var bot = container.Get<TelegramBot>();
            bot.Start();
            Console.ReadLine();
        }

        private static StandardKernel CreateContainer()
        {
            var container = new StandardKernel();
            container.Bind<Bot>().To<TelegramBot>();
            container.Bind<Bot>().To<VkBot>();

            container.Bind<TelegramBotClient>().ToConstant(new TelegramBotClient(TelegramToken));
            container.Bind<VkApi>().ToConstant(CreateVkApi().Result);

            container.Bind<IRepository>().To<DictRepository>().InSingletonScope();

            container.Bind<ICommand>().To<GreetingCommand>();
            container.Bind<ICommand>().To<CreateDeckCommand>();
            container.Bind<ICommand>().To<CreateCardCommand>();
            container.Bind<ICommand>().To<LearnDeckCommand>();

            container.Bind<IDialog>().To<CreateDeckDialog>();
            container.Bind<IDialog>().To<CreateCardDialog>();
            container.Bind<IDialog>().To<LearnDeckDialog>();

            container.Bind<ILearnMethod>().To<LineLearnMethod>().InSingletonScope();

            return container;
        }

        private static async Task<VkApi> CreateVkApi()
        {
            var api = new VkApi();
            var p = new ApiAuthParams
            {
                AccessToken = VkToken
            };
            await api.AuthorizeAsync(p);
            return api;
        }
    }
}