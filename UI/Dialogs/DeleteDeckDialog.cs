using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.UI.Commands;

namespace UI.Dialogs
{
    public class DeleteDeckDialog : IDialog
    {
        private readonly IRepository repository;

        public DeleteDeckDialog(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDialog> Execute(long userId, string message, Bot bot)
        {
            var decks = repository.GetDecksByUserId(userId.ToString());
            var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(userId, "Выберите колоду:", false);
                return this;
            }

            repository.DeleteDeck(findDeck.Id.ToString());
            await bot.SendMessage(userId, "Колода успешно удалена!");
            return null;
        }
    }
}