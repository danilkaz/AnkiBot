using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.UI.Commands;

namespace UI.Dialogs
{
    public class CreateDeckDialog : IDialog
    {
        private State state = State.ChooseDeck;
        private readonly ILearnMethod[] learnMethods;
        private readonly IRepository repository;

        private string deckName;
        private ILearnMethod deckMethod;

        public CreateDeckDialog(IRepository repository, ILearnMethod[] learnMethods)
        {
            this.repository = repository;
            this.learnMethods = learnMethods;
        }

        public async Task<IDialog> Execute(long userId, string message, IBot bot)
        {
            var keyboard = learnMethods.Select(m => new[] {m.Name}).Append(new[] {"Подробности"}).ToArray();
            if (state == State.ChooseDeck)
            {
                deckName = message;
                state = State.ChooseLearningMethod;
                await bot.SendMessageWithKeyboard(userId, "Выберите метод для запоминания", keyboard);
                return this;
            }

            if (state == State.ChooseLearningMethod)
            {
                if (message == "Подробности")
                {
                    await bot.SendMessageWithKeyboard(userId, "Подробости о методах", keyboard);
                    return this;
                }

                deckMethod = learnMethods.FirstOrDefault(m => m.Name.Equals(message));
                if (deckMethod is null)
                {
                    await bot.SendMessageWithKeyboard(userId, "Выберите метод", keyboard);
                    return this;
                }
            }

            var deck = new Deck(userId.ToString(), deckName, deckMethod);
            repository.SaveDeck(deck);
            await bot.SendMessage(userId, $"Колода успешно создана!");
            return null;
        }

        private enum State
        {
            ChooseDeck,
            ChooseLearningMethod
        }
    }
}