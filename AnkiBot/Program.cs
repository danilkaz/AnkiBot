using System;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Infrastructure;
using AnkiBot.UI.Commands;
using App;
using App.SerializedClasses;
using Infrastructure;
using Ninject;
using UI;
using UI.Config;
using UI.Dialogs;

namespace AnkiBot
{
    public static class Program
    {
        private const string PostgresConnectionString = "Host=localhost;Username=postgres;Password=postgres;" +
                                                        "Database=postgres;Port=5433";

        public static void Main()
        {
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
            container.Bind<VkConfig>().ToSelf();
            container.Bind<TelegramConfig>().ToSelf();

            container.Bind<IDatabase<DbCard>>().ToConstant(new SqLiteDatabase<DbCard>("Data source=cards.db"))
                .InSingletonScope();
            container.Bind<IDatabase<DbDeck>>().ToConstant(new SqLiteDatabase<DbDeck>("Data source=decks.db"))
                .InSingletonScope();
            // container.Bind<IDatabase<DbCard>>().ToConstant(new PostgresDatabase<DbCard>(PostgresConnectionString))
            //     .InSingletonScope();
            // container.Bind<IDatabase<DbDeck>>().ToConstant(new PostgresDatabase<DbDeck>(PostgresConnectionString))
            //     .InSingletonScope();
            container.Bind<IRepository>().To<DbRepository>().InSingletonScope();

            container.Bind<Command>().To<GreetingCommand>();
            container.Bind<Command>().To<CreateDeckCommand>();
            container.Bind<Command>().To<CreateCardCommand>();
            container.Bind<Command>().To<LearnDeckCommand>();
            container.Bind<Command>().To<DeleteDeckCommand>();
            container.Bind<Command>().To<DeleteCardCommand>();

            container.Bind<IDialog>().To<CreateDeckDialog>();
            container.Bind<IDialog>().To<CreateCardDialog>();
            container.Bind<IDialog>().To<LearnDeckDialog>();
            container.Bind<IDialog>().To<DeleteDeckDialog>();
            container.Bind<IDialog>().To<DeleteCardDialog>();

            container.Bind<ILearnMethod>().To<LineLearnMethod>().InSingletonScope();
            container.Bind<ILearnMethod>().To<SuperMemo2LearnMethod>().InSingletonScope();

            return container;
        }
    }
}