using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class ChooseDeckCommand : Command
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        public ChooseDeckCommand(DeckApi deckApi, CardApi cardApi)
        {
            this.deckApi = deckApi;
            this.cardApi = cardApi;
        }

        public override string Name => "ChooseDeckForLearn";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            var decks = deckApi.GetDecksByUser(user);
            var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
            if (findDeck is null)
            {
                await bot.SendMessage(user, "Выберите колоду:", false);
                return context;
            }

            context.DeckId = findDeck.Id;

            var learnCard = cardApi.GetCardsToLearn(context.DeckId).FirstOrDefault();
            if (learnCard is null)
            {
                await bot.SendMessage(user, "Все карточки изучены, молодец!");
                return new Context();
            }

            context.CardId = learnCard.Id;
            context.Front = learnCard.Front;
            context.Back = learnCard.Back;

            await bot.SendMessageWithKeyboard(user, learnCard.Front,
                new KeyboardProvider(new[] {new[] {"Показать ответ"}}));
            context.CommandName = "ViewFront";
            return context;
        }
    }
}