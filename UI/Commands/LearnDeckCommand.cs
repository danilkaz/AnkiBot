using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;
using UI.Dialogs;

namespace UI.Commands
{
    public class LearnDeckCommand : Command
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        public LearnDeckCommand(CardApi cardApi, DeckApi deckApi)
        {
            this.cardApi = cardApi;
            this.deckApi = deckApi;
        }

        public override string Name => "Учить колоду";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var decksNames = deckApi.GetDecksByUser(user).Select(d => d.Name);
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decksNames.Select(name => new[] {name}).ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new KeyboardProvider(decksKeyboard));
            return new LearnDeckDialog(cardApi, deckApi);
        }
    }
}