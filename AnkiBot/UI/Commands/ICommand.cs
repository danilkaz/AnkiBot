using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiBot.UI.Commands
{
    interface ICommand
    {
        string Name { get; set; }

        public void Execute(string message);
    }
}
