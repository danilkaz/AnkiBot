using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class DeleteDeckCommand : ICommand
    {
        public string Name => "Удалить колоду";
        private readonly IRepository repository;

        public DeleteDeckCommand(IRepository repository, ILearnMethod[] learnMethods)
        {
            this.repository = repository;
        }
        
        public async Task<IDialog> Execute(long userId, string message, IBot bot)
        {
            var decks = repository.GetDecksByUserId(userId.ToString());
            if (!decks.Any())
            {
                await bot.SendMessage(userId, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }
            var decksKeyboard = decks
                .Select(deck => new[] {deck.Name})
                .ToArray();
            await bot.SendMessageWithKeyboard(userId, "Выберите колоду:", decksKeyboard);
            return new DeleteDeckDialog(repository);
        }
    }
}