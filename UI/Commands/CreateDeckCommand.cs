using System;
using System.Threading;
using System.Threading.Tasks;
using AnkiBot.Domain;

namespace AnkiBot.UI.Commands
{
    public class CreateDeckCommand : ICommand
    {
        public string Name => "Создать колоду";
        private readonly State state;
        private CreateDeckCommand(State state)
        {
            this.state = state;
        }

        public CreateDeckCommand()
        {
            this.state = State.Start;
        }

        public async Task<ICommand> Execute(long userId, string message, IBot bot)
        {
            switch (state)
            {
                case State.Start:
                    await bot.SendMessage(userId, "Введите имя колоды");
                    return new CreateDeckCommand(State.GetDeckName);
                case State.GetDeckName:
                    var deck = new Deck(userId.ToString(), message);
                    await bot.SendMessage(userId, deck.Id.ToString());
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum State
        {
            Start,
            GetDeckName,
        }
    }
}