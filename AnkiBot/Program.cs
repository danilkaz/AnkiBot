using System;
using AnkiBot.UI;
using AnkiBot.UI.Commands;
using Telegram.Bot;
using Ninject;
using VkNet;


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
            
            container.Bind<string>().ToConstant(TelegramToken);
            container.Bind<ICommand>().To<GreetingCommand>();
            container.Bind<ICommand>().To<CreateDeckCommand>();

            return container.Get<TelegramBot>();
        }
    }
}