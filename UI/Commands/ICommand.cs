﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiBot.UI.Commands
{
    public interface ICommand
    {
        string Name { get; }
        Task<ICommand> Execute(long userId, string message, IBot bot);
    }
}