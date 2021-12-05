using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class CreateDeckCommand : ICommand
    {
        public string Name => "Создать колоду";
        private readonly IRepository repository;
        private readonly ILearnMethod[] learnMethods;

        public CreateDeckCommand(IRepository repository, ILearnMethod[] learnMethods)
        {
            this.repository = repository;
            this.learnMethods = learnMethods;
        }
        public async Task<IDialog> Execute(long userId, string message, IBot bot)
        {
            await bot.SendMessage(userId, "Введите имя колоды");
            return new CreateDeckDialog(repository, learnMethods);
        }
    }
}