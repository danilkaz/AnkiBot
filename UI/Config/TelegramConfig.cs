using System;

namespace UI.Config
{
    public class TelegramConfig
    {
        public string Token => Environment.GetEnvironmentVariable("TELEGRAM_TOKEN", EnvironmentVariableTarget.User);
    }
}