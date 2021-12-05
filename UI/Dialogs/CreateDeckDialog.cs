using System;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.UI.Commands;
using Telegram.Bot.Requests;

namespace UI.Dialogs
{
    public class CreateDeckDialog : IDialog
    {
        private State state = State.InputDeckName;
        // тут должен быть список методов изучения

        private readonly IRepository repository;

        private string deckName;
        private string deckMethod;

        public CreateDeckDialog(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDialog> Execute(long userId, string message, IBot bot)
        {
            var availableMethods = new[] {"Метод 1", "Метод 2"};

            var keyboard = new[] {new[] {"Метод 1"}, new[] {"Метод 2"}, new[] {"Подробности"}};
            if (state == State.InputDeckName)
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

                if (message is "Метод 1" or "Метод 2") // проверка на выбор метода
                {
                    deckMethod = message;
                }
                else
                {
                    await bot.SendMessageWithKeyboard(userId, "Выберите метод", keyboard);
                    return this;
                }
            }

            var deck = new Deck(userId.ToString(), deckName);
            repository.SaveDeck(deck);
            await bot.SendMessage(userId, $"Колода успешно создана!");
            return null;
        }

        private enum State
        {
            InputDeckName,
            ChooseLearningMethod
        }
    }
}