using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.DeleteDeckCommands
{
    public class ChooseDeckCommand : ICommand
    {
        private readonly DeckApi deckApi;

        public ChooseDeckCommand(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public string Name => "ChooseDeckForDeleteDeck";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var decksName = deckApi.GetDecksByUser(user);
            var findDeck = decksName.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return ICommandInfo.Create<ChooseDeckCommand>();
            }

            deckApi.DeleteDeck(findDeck.Id);
            await bot.SendMessage(user, "Колода успешно удалена!");
            await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
            return ICommandInfo.Create<StartCommand>();
        }
    }
}