using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;

namespace UI.Commands.DeleteCardCommands
{
    public class ChooseDeckCommand : Command
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        public ChooseDeckCommand(DeckApi deckApi, CardApi cardApi)
        {
            this.deckApi = deckApi;
            this.cardApi = cardApi;
        }

        public override string Name => "ChooseDeckForDeleteCard";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            var decks = deckApi.GetDecksByUser(user);
            var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return context;
            }

            context.DeckId = findDeck.Id;

            var cards = cardApi.GetCardsByDeckId(context.DeckId);
            if (!cards.Any())
            {
                await bot.SendMessage(user, "Колода пуста", false);
                return null;
            }

            var cardsKeyboard = cards.Select(c => new[] {c.Front + "\n" + c.Id}).ToArray();

            await bot.SendMessageWithKeyboard(user, "Выберите карту:", new KeyboardProvider(cardsKeyboard));
            context.CommandName = "ChooseCard";
            return context;
        }
    }
}