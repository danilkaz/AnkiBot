using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.DeleteDeckCommands
{
    public class InitialDeleteDeckCommand : ICommand
    {
        private readonly DeckApi deckApi;

        public InitialDeleteDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public string Name => "Удалить колоду";
        public bool IsInitial => true;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var decksNames = deckApi.GetDecksByUser(user).ToArray();
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                return ICommandInfo.Create<StartCommand>();
            }

            var decksKeyboard = decksNames
                .Select(d => new[] {d.Name})
                .ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new(decksKeyboard));
            return ICommandInfo.Create<ChooseDeckCommand>();
        }
    }
}