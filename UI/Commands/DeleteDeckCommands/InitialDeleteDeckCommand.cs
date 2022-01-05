using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.DeleteDeckCommands
{
    public class InitialDeleteDeckCommand : Command
    {
        private readonly DeckApi deckApi;

        public InitialDeleteDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public override string Name => "Удалить колоду";
        public override bool isInitial => true;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
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
            context.CommandName = "ChooseDeckForDeleteDeck";
            return context;
        }
    }
}