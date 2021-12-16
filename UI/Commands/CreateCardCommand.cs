using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using UI;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class CreateCardCommand : Command
    {
        private readonly IRepository repository;

        public CreateCardCommand(IRepository repository)
        {
            this.repository = repository;
        }

        public override string Name => "Добавить карточку";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var decksNames = repository.GetDecksNamesByUser(user);
            if (!decksNames.Any())
            {
                await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
                return null;
            }

            var decksKeyboard = decksNames
                .Select(name => new[] {name})
                .ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new KeyboardProvider(decksKeyboard));
            return new CreateCardDialog(repository);
        }
    }
}