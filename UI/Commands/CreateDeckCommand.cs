using System.Threading.Tasks;
using App;
using Domain;
using Domain.LearnMethods;
using UI.Dialogs;

namespace UI.Commands
{
    public class CreateDeckCommand : Command
    {
        private readonly Converter converter;
        private readonly ILearnMethod[] learnMethods;
        private readonly IRepository repository;

        public CreateDeckCommand(IRepository repository, ILearnMethod[] learnMethods, Converter converter)
        {
            this.repository = repository;
            this.learnMethods = learnMethods;
            this.converter = converter;
        }

        public override string Name => "Создать колоду";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            await bot.SendMessage(user, "Введите имя колоды");
            return new CreateDeckDialog(repository, learnMethods, converter);
        }
    }
}