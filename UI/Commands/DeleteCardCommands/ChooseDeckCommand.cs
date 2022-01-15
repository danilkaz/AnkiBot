using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.DeleteCardCommands
{
    public class ChooseDeckCommand : ICommand
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        public ChooseDeckCommand(DeckApi deckApi, CardApi cardApi)
        {
            this.deckApi = deckApi;
            this.cardApi = cardApi;
        }

        public string Name => "ChooseDeckForDeleteCard";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var decks = deckApi.GetDecksByUser(user);
            var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return ICommandInfo.Create<ChooseDeckCommand>();
            }

            var data = new ChooseCardData(findDeck.Id);

            var cards = cardApi.GetCardsByDeckId(findDeck.Id).ToArray();
            if (!cards.Any())
            {
                await bot.SendMessage(user, "Колода пуста", false);
                await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                return ICommandInfo.Create<StartCommand>();
            }

            var cardsKeyboard = cards.Select(c => new[] { c.Front + "\n" + c.Id }).ToArray();

            await bot.SendMessageWithKeyboard(user, "Выберите карту:", new(cardsKeyboard));
            return ICommandInfo.Create<ChooseCardData, ChooseCardCommand>(data);
        }
    }
}