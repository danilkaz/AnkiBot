using System;
using System.Threading;
using App;
using App.APIs;
using App.Converters;
using App.SerializedClasses;
using App.UIClasses;
using Domain;
using Domain.LearnMethods;
using Infrastructure;
using Ninject;
using Ninject.Extensions.Conventions;
using Npgsql;
using UI;
using UI.Commands;
using UI.Commands.CreateCardCommands;
using UI.Commands.CreateDeckCommands;
using UI.Commands.DeleteCardCommands;
using UI.Commands.DeleteDeckCommands;
using UI.Commands.LearnDeckCommands;
using UI.Config;
using ChooseDeckCommand = UI.Commands.CreateCardCommands.ChooseDeckCommand;

namespace AnkiBot
{
    public static class Program
    {
        private const string SqliteConnectionString = "Data source=db.db";

        private static readonly string PostgresConnectionString = GetPostgresConnectionString();

        private static readonly string Database =
            Environment.GetEnvironmentVariable("BOT_DATABASE");

        public static void Main()
        {
            using var container = CreateContainer();

            container.Get<IDatabase<DbCard>>().CreateTable();
            container.Get<IDatabase<DbDeck>>().CreateTable();
            container.Get<IDatabase<DbContext>>().CreateTable();

            foreach (var bot in container.GetAll<IBot>())
                new Thread(bot.Start).Start();
            while (true)
                Console.ReadLine();
        }

        private static StandardKernel CreateContainer()
        {
            var container = new StandardKernel();

            container.Bind<VkConfig>().ToSelf().InSingletonScope();
            container.Bind<TelegramConfig>().ToSelf().InSingletonScope();

            if (Database == "Sqlite")
            {
                container.Bind<IDatabase<DbCard>>().To<SqLiteDatabase<DbCard>>().InSingletonScope()
                    .WithConstructorArgument(SqliteConnectionString);
                container.Bind<IDatabase<DbDeck>>().To<SqLiteDatabase<DbDeck>>().InSingletonScope()
                    .WithConstructorArgument(SqliteConnectionString);
                container.Bind<IDatabase<DbContext>>().To<SqLiteDatabase<DbContext>>().InSingletonScope()
                    .WithConstructorArgument(SqliteConnectionString);
            }
            else
            {
                container.Bind<NpgsqlConnection>().ToSelf().WithConstructorArgument(PostgresConnectionString);
                container.Bind<IDatabase<DbCard>>().To<PostgresDatabase<DbCard>>().InSingletonScope();
                container.Bind<IDatabase<DbDeck>>().To<PostgresDatabase<DbDeck>>().InSingletonScope();
                container.Bind<IDatabase<DbContext>>().To<PostgresDatabase<DbContext>>().InSingletonScope()
                    .WithConstructorArgument(PostgresConnectionString);
            }

            container.Bind<IRepository<DbCard>>().To<CardRepository>().InSingletonScope();
            container.Bind<IRepository<DbDeck>>().To<DeckRepository>().InSingletonScope();
            container.Bind<IRepository<DbContext>>().To<ContextRepository>().InSingletonScope();

            container.Bind<IConverter<DbCard, UICard, Card>>().To<CardConverter>().InSingletonScope();
            container.Bind<IConverter<DbDeck, UIDeck, Deck>>().To<DeckConverter>().InSingletonScope();

            container.Bind<CardApi>().ToSelf().InSingletonScope();
            container.Bind<DeckApi>().ToSelf().InSingletonScope();
            container.Bind<ContextApi>().ToSelf().InSingletonScope();

            container.Bind<ICommand>().To<GreetingCommand>().InSingletonScope();
            container.Bind<ICommand>().To<StartCommand>().InSingletonScope();

            container.Bind<ICommand>().To<InitialCreateCardCommand>().InSingletonScope();
            container.Bind<ICommand>().To<ChooseDeckCommand>().InSingletonScope();

            container.Bind<ICommand>().To<InitialCreateDeckCommand>().InSingletonScope();
            container.Bind<ICommand>().To<InputDeckNameCommand>().InSingletonScope();

            container.Bind<ICommand>().To<InitialDeleteCardCommand>().InSingletonScope();
            container.Bind<ICommand>().To<UI.Commands.DeleteCardCommands.ChooseDeckCommand>().InSingletonScope();

            container.Bind<ICommand>().To<InitialDeleteDeckCommand>().InSingletonScope();
            container.Bind<ICommand>().To<UI.Commands.DeleteDeckCommands.ChooseDeckCommand>().InSingletonScope();

            container.Bind<ICommand>().To<InitialLearnDeckCommand>().InSingletonScope();
            container.Bind<ICommand>().To<UI.Commands.LearnDeckCommands.ChooseDeckCommand>().InSingletonScope();


            container.Bind<ICommandFactory<InputFrontData, InputFrontCommand>>().To<InputFrontCommandFactory>()
                .InSingletonScope();
            container.Bind<ICommandFactory<InputBackData, InputBackCommand>>().To<InputBackCommandFactory>()
                .InSingletonScope();
            container.Bind<ICommandFactory<ChooseLearnMethodData, ChooseLearnMethodCommand>>()
                .To<ChooseLearnMethodCommandFactory>().InSingletonScope();
            container.Bind<ICommandFactory<ChooseCardData, ChooseCardCommand>>().To<ChooseCardCommandFactory>()
                .InSingletonScope();
            container.Bind<ICommandFactory<ViewFrontData, ViewFrontCommand>>().To<ViewFrontCommandFactory>()
                .InSingletonScope();
            container.Bind<ICommandFactory<ViewBackData, ViewBackCommand>>().To<ViewBackCommandFactory>()
                .InSingletonScope();

            container.Bind(c =>
                c.FromAssemblyContaining<ILearnMethod>().SelectAllClasses().InheritedFrom<ILearnMethod>()
                    .BindAllInterfaces());

            container.Bind<BotHandler>().ToSelf().InSingletonScope();
            container.Bind<IBot>().To<TelegramBot>().InSingletonScope();
            container.Bind<IBot>().To<VkBot>().InSingletonScope();

            container.Bind<StandardKernel>().ToConstant(container);
            return container;
        }

        private static string GetPostgresConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            return $"Host={host};Username={username};Password={password};Database={database};Port={port}";
        }
    }
}


//TODO: пофиксить карточки (чтобы плохо изученные карточки появлялись вновь)