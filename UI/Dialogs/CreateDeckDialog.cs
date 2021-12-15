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
        private readonly ILearnMethod[] learnMethods;
        private readonly IRepository repository;
        private ILearnMethod deckMethod;

        private string deckName;
        private State state = State.ChooseDeck;

        public CreateDeckDialog(IRepository repository, ILearnMethod[] learnMethods)
        {
            this.repository = repository;
            this.learnMethods = learnMethods;
        }

        public async Task<IDialog> Execute(long userId, string message, Bot bot)
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
                    foreach (var method in learnMethods)
                        await bot.SendMessageWithKeyboard(userId, method.Description, keyboard);
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
            await bot.SendMessage(userId, "Колода успешно создана!");
            return null;
        }

        private enum State
        {
            ChooseDeck,
            ChooseLearningMethod
        }
    }
}