﻿using System;
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

            container.Get<IDatabase<DbCard>>().CreateTable("Data source=cards.db");
            container.Get<IDatabase<DbDeck>>().CreateTable("Data source=decks.db");
            // container.Get<IDatabase<DbCard>>().CreateTable(PostgresConnectionString);
            // container.Get<IDatabase<DbDeck>>().CreateTable(PostgresConnectionString);

            var bot = container.Get<TelegramBot>();
            bot.Start();
            Console.ReadLine();
        }

        private static StandardKernel CreateContainer()
        {
            var container = new StandardKernel();

            container.Bind<VkConfig>().ToSelf();
            container.Bind<TelegramConfig>().ToSelf();
            
            container.Bind<IDatabase<DbCard>>().To<SqLiteDatabase<DbCard>>().InSingletonScope();
            container.Bind<IDatabase<DbDeck>>().To<SqLiteDatabase<DbDeck>>().InSingletonScope();
            // container.Bind<IDatabase<DbCard>>().To<PostgresDatabase<DbCard>>().InSingletonScope();
            // container.Bind<IDatabase<DbDeck>>().To<PostgresDatabase<DbDeck>>().InSingletonScope();
            
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

            container.Bind<Bot>().ToSelf();
            container.Bind<IBot>().To<TelegramBot>();
            container.Bind<IBot>().To<VkBot>();

            return container;
        }
    }
}

// TODO: создать классы для представления в UI