using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class InitialLearnDeckCommand : ICommand
    {
        private readonly DeckApi deckApi;

        public InitialLearnDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public string Name => "Учить колоду";
        public bool IsInitial => true;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var decksNames = deckApi.GetDecksByUser(user).Select(d => d.Name).ToArray();
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decksNames.Select(name => new[] { name }).ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new(decksKeyboard));
            return ICommandInfo.Create<ChooseDeckCommand>();
        }
    }
}