using System.Collections.Generic;
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
        private IEnumerable<Deck> userDecks;

        public CreateDeckDialog(IRepository repository, ILearnMethod[] learnMethods)
        {
            this.repository = repository;
            this.learnMethods = learnMethods;
        }

        public async Task<IDialog> Execute(User user, string message, Bot bot)
        {
            userDecks ??= repository.GetDecksByUser(user);
            var keyboard = learnMethods.Select(m => new[] {m.Name}).Append(new[] {"Подробности"}).ToArray();
            if (state == State.ChooseDeck)
            {
                if (userDecks.FirstOrDefault(d => d.Name == message) is not null)
                {
                    await bot.SendMessage(user, "Колода с таким именем уже создана");
                    await bot.SendMessage(user, "Введите имя колоды");
                    return this;
                }
                deckName = message;
                state = State.ChooseLearningMethod;
                await bot.SendMessageWithKeyboard(user, "Выберите метод для запоминания", keyboard);
                return this;
            }

            if (state == State.ChooseLearningMethod)
            {
                if (message == "Подробности")
                {
                    foreach (var method in learnMethods)
                        await bot.SendMessageWithKeyboard(user, method.Description, keyboard);
                    return this;
                }

                deckMethod = learnMethods.FirstOrDefault(m => m.Name.Equals(message));
                if (deckMethod is null)
                {
                    await bot.SendMessageWithKeyboard(user, "Выберите метод", keyboard);
                    return this;
                }
            }

            var deck = new Deck(user, deckName, deckMethod);
            repository.SaveDeck(deck);
            await bot.SendMessage(user, "Колода успешно создана!");
            return null;
        }

        private enum State
        {
            ChooseDeck,
            ChooseLearningMethod
        }
    }
}