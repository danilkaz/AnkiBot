using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.CreateCardCommands
{
    public class InitialCreateCardCommand : ICommand
    {
        private readonly DeckApi deckApi;

        public InitialCreateCardCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public string Name => "Добавить карточку";
        public bool IsInitial => true;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var decksNames = deckApi.GetDecksByUser(user).ToArray();
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decksNames
                .Select(d => new[] {d.Name})
                .ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new(decksKeyboard));
            return ICommandInfo.Create<ChooseDeckCommand>();
        }
    }
}