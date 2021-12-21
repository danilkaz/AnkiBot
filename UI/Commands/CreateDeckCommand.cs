using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using App;
using UI.Dialogs;

namespace AnkiBot.UI.Commands
{
    public class CreateDeckCommand : Command
    {
        private readonly ILearnMethod[] learnMethods;
        private readonly IRepository repository;
        private readonly Converter converter; 

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