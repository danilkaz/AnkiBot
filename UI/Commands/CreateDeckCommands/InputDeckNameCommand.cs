using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;
using Domain.LearnMethods;

namespace UI.Commands.CreateDeckCommands
{
    public class InputDeckNameCommand : ICommand
    {
        private readonly DeckApi deckApi;
        private readonly ILearnMethod[] learnMethods;

        public InputDeckNameCommand(DeckApi deckApi, ILearnMethod[] learnMethods)
        {
            this.deckApi = deckApi;
            this.learnMethods = learnMethods;
        }

        public string Name => "InputDeckName";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var deck = deckApi.GetDecksByUser(user).FirstOrDefault(d => d.Name == message);

            if (deck != null)
            {
                await bot.SendMessage(user, "Колода с таким именем уже создана");
                await bot.SendMessage(user, "Введите имя колоды");
                return ICommandInfo.Create<InputDeckNameCommand>();
            }

            var data = new ChooseLearnMethodData(message);
            var keyboard = learnMethods.Select(m => new[] { m.Name }).Append(new[] { "Подробности" }).ToArray();
            await bot.SendMessageWithKeyboard(user, "Выберите метод для запоминания",
                new(keyboard));
            return ICommandInfo.Create<ChooseLearnMethodData, ChooseLearnMethodCommand>(data);
        }
    }
}