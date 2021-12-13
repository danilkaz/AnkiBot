using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain.LearnMethods;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class CreateDeckCommand : Command
    {
        private readonly ILearnMethod[] learnMethods;
        private readonly IRepository repository;

        public CreateDeckCommand(IRepository repository, ILearnMethod[] learnMethods)
        {
            this.repository = repository;
            this.learnMethods = learnMethods;
        }

        public override string Name => "Создать колоду";

        public override async Task<IDialog> Execute(long userId, string message, Bot bot)
        {
            await bot.SendMessage(userId, "Введите имя колоды");
            return new CreateDeckDialog(repository, learnMethods);
        }
    }
}