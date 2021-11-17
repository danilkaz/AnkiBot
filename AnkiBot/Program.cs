using System;
using AnkiBot.UI;
using AnkiBot.UI.Commands;
using Telegram.Bot;
using Ninject;
using Ninject.Extensions.Conventions;


namespace AnkiBot
{
    public static class Program
    {
        private static readonly string Token =
            Environment.GetEnvironmentVariable("BOT_TOKEN", EnvironmentVariableTarget.User);

        public static void Main()
        {
            var bot = CreateTelegramBot();
            bot.Start();
        }

        private static TelegramBot CreateTelegramBot()
        {
            var container = new StandardKernel();
            
            container.Bind<TelegramBotClient>().ToConstant(new TelegramBotClient(Token));
            container.Bind<ICommand>().To<GreetingCommand>();

            return container.Get<TelegramBot>();
        }
    }
}