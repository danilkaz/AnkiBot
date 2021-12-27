using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;

namespace UI.Dialogs
{
    public class DeleteDeckDialog : IDialog
    {
        private readonly DeckApi deckApi;

        public DeleteDeckDialog(DeckApi deckApi)
        {
            this.deckApi = deckApi;
        }

        public async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var decksName = deckApi.GetDecksByUser(user);
            var findDeck = decksName.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return this;
            }

            deckApi.DeleteDeck(findDeck.Id);
            await bot.SendMessage(user, "Колода успешно удалена!");
            return null;
        }
    }
}