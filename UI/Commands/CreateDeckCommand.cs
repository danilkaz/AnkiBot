using System.Threading.Tasks;
using App;
using Domain;
using Domain.LearnMethods;
using UI.Dialogs;

namespace UI.Commands
{
    public class CreateDeckCommand : Command
    {
        private readonly DeckApi deckApi;
        private readonly ILearnMethod[] learnMethods;

        public CreateDeckCommand(ILearnMethod[] learnMethods, DeckApi deckApi)
        {
            this.learnMethods = learnMethods;
            this.deckApi = deckApi;
        }

        public override string Name => "Создать колоду";

        public override async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            await bot.SendMessage(user, "Введите имя колоды");
            return new CreateDeckDialog(learnMethods, deckApi);
        }
    }
}