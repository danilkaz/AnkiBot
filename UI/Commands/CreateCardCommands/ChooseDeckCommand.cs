using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.CreateCardCommands
{
    public class ChooseDeckCommand : Command
    {
        private readonly DeckApi deckApi;
        private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};

        public ChooseDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public override string Name => "ChooseDeckForCreateCard";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            var decks = deckApi.GetDecksByUser(user);
            var deck = decks.FirstOrDefault(d => d.Name == message);
            if (deck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return context;
            }

            context.DeckId = deck.Id;
            context.CommandName = "InputFront";
            await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
                new KeyboardProvider(finishKeyboard));
            return context;
        }
    }
}