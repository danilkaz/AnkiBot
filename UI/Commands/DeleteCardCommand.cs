using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class DeleteCardCommand : Command
    {
        private readonly IRepository repository;

        public DeleteCardCommand(IRepository repository)
        {
            this.repository = repository;
        }

        public override string Name => "Удалить карточку";

        public override async Task<IDialog> Execute(User user, string message, Bot bot)
        {
            var decks = repository.GetDecksByUser(user);
            if (!decks.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decks
                .Select(deck => new[] {deck.Name})
                .ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", decksKeyboard);
            return new DeleteCardDialog(repository);
        }
    }
}