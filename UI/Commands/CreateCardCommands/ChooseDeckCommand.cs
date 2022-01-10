using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.CreateCardCommands
{
    public class ChooseDeckCommand : ICommand
    {
        private readonly DeckApi deckApi;
        private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};

        public ChooseDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public string Name => "ChooseDeckForCreateCard";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var decks = deckApi.GetDecksByUser(user);
            var deck = decks.FirstOrDefault(d => d.Name == message);
            if (deck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return ICommandInfo.Create<ChooseDeckCommand>();
            }

            var data = new InputFrontData(deck.Id);

            await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
                new(finishKeyboard));
            return ICommandInfo.Create<InputFrontData, InputFrontCommand>(data);
        }
    }
}