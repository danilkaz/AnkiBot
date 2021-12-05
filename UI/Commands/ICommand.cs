using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public interface ICommand
    {
        string Name { get; }
        Task<IDialog> Execute(long userId, string message, IBot bot);
    }
}
