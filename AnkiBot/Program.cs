using System;
using System.Reflection;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Infrastructure;
using AnkiBot.UI;
using AnkiBot.UI.Commands;
using App;
using App.SerializedClasses;
using Infrastructure;
using Microsoft.Data.Sqlite;
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
            container.Bind<IDatabase<DbCard>>().ToConstant(new SqLiteDatabase<DbCard>("Data Source=cards.db")).InSingletonScope();
            container.Bind<IDatabase<DbDeck>>().ToConstant(new SqLiteDatabase<DbDeck>("Data Source=decks.db")).InSingletonScope();
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