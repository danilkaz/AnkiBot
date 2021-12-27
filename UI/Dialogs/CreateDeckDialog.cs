using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;
using Domain.LearnMethods;

namespace UI.Dialogs
{
    public class CreateDeckDialog : IDialog
    {
        private readonly DeckApi deckApi;
        private readonly ILearnMethod[] learnMethods;
        private ILearnMethod deckMethod;

        private string deckName;
        private IEnumerable<string> decksNames;
        private State state = State.ChooseDeck;

        public CreateDeckDialog(ILearnMethod[] learnMethods, DeckApi deckApi)
        {
            this.learnMethods = learnMethods;
            this.deckApi = deckApi;
        }

        public async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            decksNames ??= deckApi.GetDecksByUser(user).Select(d => d.Name);
            var keyboard = learnMethods.Select(m => new[] {m.Name}).Append(new[] {"Подробности"}).ToArray();
            if (state == State.ChooseDeck)
            {
                if (decksNames.FirstOrDefault(name => name == message) is not null)
                {
                    await bot.SendMessage(user, "Колода с таким именем уже создана");
                    await bot.SendMessage(user, "Введите имя колоды");
                    return this;
                }

                deckName = message;
                state = State.ChooseLearningMethod;
                await bot.SendMessageWithKeyboard(user, "Выберите метод для запоминания",
                    new KeyboardProvider(keyboard));
                return this;
            }

            if (state == State.ChooseLearningMethod)
            {
                if (message == "Подробности")
                {
                    foreach (var method in learnMethods)
                        await bot.SendMessageWithKeyboard(user, method.Description, new KeyboardProvider(keyboard));
                    return this;
                }

                deckMethod = learnMethods.FirstOrDefault(m => m.Name.Equals(message));
                if (deckMethod is null)
                {
                    await bot.SendMessageWithKeyboard(user, "Выберите метод", new KeyboardProvider(keyboard));
                    return this;
                }
            }

            deckApi.SaveDeck(user, deckName, deckMethod, new List<Card>());
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