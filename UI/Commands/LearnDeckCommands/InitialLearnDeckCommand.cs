using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class InitialLearnDeckCommand : Command
    {
        private readonly DeckApi deckApi;

        public InitialLearnDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public override string Name => "Учить колоду";
        public override bool isInitial => true;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            var decksNames = deckApi.GetDecksByUser(user).Select(d => d.Name);
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decksNames.Select(name => new[] {name}).ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new KeyboardProvider(decksKeyboard));
            context.CommandName = "ChooseDeckForLearn";
            return context;
        }
    }
}