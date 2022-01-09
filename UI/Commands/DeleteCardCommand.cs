using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;
using UI.Dialogs;

namespace UI.Commands
{
    public class DeleteCardCommand : Command
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        public DeleteCardCommand(DeckApi deckApi, CardApi cardApi)
        {
            this.deckApi = deckApi;
            this.cardApi = cardApi;
        }

        public override string Name => "Удалить карточку";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var decksNames = deckApi.GetDecksByUser(user);
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decksNames
                .Select(d => new[] { d.Name })
                .ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new KeyboardProvider(decksKeyboard));
            return new DeleteCardDialog(cardApi, deckApi);
        }
    }
}