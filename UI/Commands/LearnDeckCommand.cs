using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class LearnDeckCommand : Command
    {
        private readonly IRepository repository;

        public LearnDeckCommand(IRepository repository)
        {
            this.repository = repository;
        }

        public override string Name => "Учить колоду";

        public override async Task<IDialog> Execute(long userId, string message, Bot bot)
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
            return new LearnDeckDialog(repository);
        }
    }
}