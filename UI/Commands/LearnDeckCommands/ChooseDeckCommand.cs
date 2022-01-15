using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class ChooseDeckCommand : ICommand
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        public ChooseDeckCommand(DeckApi deckApi, CardApi cardApi)
        {
            this.deckApi = deckApi;
            this.cardApi = cardApi;
        }

        public string Name => "ChooseDeckForLearn";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var decks = deckApi.GetDecksByUser(user);
            var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return ICommandInfo.Create<ChooseDeckCommand>();
            }

            var learnCard = cardApi.GetCardsToLearn(findDeck.Id).FirstOrDefault();
            if (learnCard is null)
            {
                await bot.SendMessage(user, "Все карточки изучены, молодец!");
                return ICommandInfo.Create<StartCommand>();
            }

            var data = new ViewFrontData(findDeck.Id, learnCard.Id, learnCard.Front, learnCard.Back);

            await bot.SendMessageWithKeyboard(user, learnCard.Front,
                new(new[] { new[] { "Показать ответ" } }));
            return ICommandInfo.Create<ViewFrontData, ViewFrontCommand>(data);
        }
    }
}