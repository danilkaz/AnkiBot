using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.DeleteCardCommands
{
    public class InitialDeleteCardCommand : Command
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        public InitialDeleteCardCommand(DeckApi deckApi, CardApi cardApi)
        {
            this.deckApi = deckApi;
            this.cardApi = cardApi;
        }

        public override string Name => "Удалить карточку";
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
            context.CommandName = "ChooseDeckForDeleteCard";
            return context;
        }
    }
}