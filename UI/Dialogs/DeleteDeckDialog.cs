using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;

namespace UI.Dialogs
{
    public class DeleteDeckDialog : IDialog
    {
        private readonly IRepository repository;

        public DeleteDeckDialog(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var decksName = repository.GetDecksByUser(user);
            var findDeck = decksName.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return this;
            }

            repository.DeleteDeck(findDeck.Id.ToString());
            await bot.SendMessage(user, "Колода успешно удалена!");
            return null;
        }
    }
}