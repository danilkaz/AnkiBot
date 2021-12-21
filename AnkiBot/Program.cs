using System;
using System.Threading;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.Infrastructure;
using AnkiBot.UI.Commands;
using App;
using App.SerializedClasses;
using Infrastructure;
using Ninject;
using Ninject.Extensions.Conventions;
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
            using var container = CreateContainer();

            container.Get<IDatabase<DbCard>>().CreateTable("Data source=db.db");
            container.Get<IDatabase<DbDeck>>().CreateTable("Data source=db.db");
            // container.Get<IDatabase<DbCard>>().CreateTable(PostgresConnectionString);
            // container.Get<IDatabase<DbDeck>>().CreateTable(PostgresConnectionString);

            new Thread(container.Get<VkBot>().Start).Start();
            new Thread(container.Get<TelegramBot>().Start).Start();
            Console.ReadLine();
        }

        private static StandardKernel CreateContainer()
        {
            var container = new StandardKernel();

            container.Bind<VkConfig>().ToSelf().InSingletonScope();
            container.Bind<TelegramConfig>().ToSelf().InSingletonScope();

            container.Bind<IDatabase<DbCard>>().To<SqLiteDatabase<DbCard>>().InSingletonScope();
            container.Bind<IDatabase<DbDeck>>().To<SqLiteDatabase<DbDeck>>().InSingletonScope();
            // container.Bind<IDatabase<DbCard>>().To<PostgresDatabase<DbCard>>().InSingletonScope();
            // container.Bind<IDatabase<DbDeck>>().To<PostgresDatabase<DbDeck>>().InSingletonScope();

            container.Bind<IRepository>().To<DbRepository>().InSingletonScope();

            container.Bind<Converter>().ToSelf().InSingletonScope();
            
            container.Bind(c =>
                c.FromAssemblyContaining<Command>().SelectAllClasses().InheritedFrom<Command>().BindAllBaseClasses());
            container.Bind(c =>
                c.FromAssemblyContaining<IDialog>().SelectAllClasses().InheritedFrom<IDialog>().BindAllInterfaces());
            container.Bind(c =>
                c.FromAssemblyContaining<ILearnMethod>().SelectAllClasses().InheritedFrom<ILearnMethod>()
                    .BindAllInterfaces());

            container.Bind<BotHandler>().ToSelf();
            container.Bind<IBot>().To<TelegramBot>().InSingletonScope();
            container.Bind<IBot>().To<VkBot>().InSingletonScope();

            return container;
        }
    }
}

// TODO: создать классы для представления в UI