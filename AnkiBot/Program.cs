using System;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Infrastructure;
using AnkiBot.UI;
using AnkiBot.UI.Commands;
using App;
using App.SerializedClasses;
using Infrastructure;
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
    
        private const string SqLiteConnectionString = "Data source=anki.db";
        
        private const string PostgresConnectionString = "Host=localhost;Username=postgres;Password=postgres;" +
                                                        "Database=postgres;Port=5433";
        public static void Main()
        {
            var container = CreateContainer();
            var bot = container.Get<VkBot>();
            bot.Start();
            Console.ReadLine();
        }

        private static StandardKernel CreateContainer()
        {
            var container = new StandardKernel();
            container.Bind<Bot>().To<TelegramBot>();
            container.Bind<Bot>().To<VkBot>();
            container.Bind<VkApi>().ToConstant(CreateVkApi().Result);

            container.Bind<TelegramBotClient>().ToConstant(new TelegramBotClient(TelegramToken));
            // container.Bind<IDatabase<DbCard>>().ToConstant(new SqLiteDatabase<DbCard>(SqLiteConnectionString))
            //     .InSingletonScope();
            // container.Bind<IDatabase<DbDeck>>().ToConstant(new SqLiteDatabase<DbDeck>(SqLiteConnectionString))
            //     .InSingletonScope();
            container.Bind<IDatabase<DbCard>>().ToConstant(new PostgresDatabase<DbCard>(PostgresConnectionString))
                .InSingletonScope();
            container.Bind<IDatabase<DbDeck>>().ToConstant(new PostgresDatabase<DbDeck>(PostgresConnectionString))
                .InSingletonScope();
            container.Bind<IRepository>().To<DbRepository>().InSingletonScope();

            container.Bind<ICommand>().To<GreetingCommand>();
            container.Bind<ICommand>().To<CreateDeckCommand>();
            container.Bind<ICommand>().To<CreateCardCommand>();
            container.Bind<ICommand>().To<LearnDeckCommand>();
            container.Bind<ICommand>().To<DeleteDeckCommand>();
            container.Bind<ICommand>().To<DeleteCardCommand>();

            container.Bind<IDialog>().To<CreateDeckDialog>();
            container.Bind<IDialog>().To<CreateCardDialog>();
            container.Bind<IDialog>().To<LearnDeckDialog>();
            container.Bind<IDialog>().To<DeleteDeckDialog>();
            container.Bind<IDialog>().To<DeleteCardDialog>();

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