using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;
using UI.Dialogs;

namespace UI.Commands
{
    public class DeleteDeckCommand : Command
    {
        private readonly DeckApi deckApi;

        public DeleteDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public override string Name => "Удалить колоду";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var decksNames = deckApi.GetDecksByUser(user);
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decksNames
                .Select(d => new[] {d.Name})
                .ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new KeyboardProvider(decksKeyboard));
            return new DeleteDeckDialog(deckApi);
        }
    }
}