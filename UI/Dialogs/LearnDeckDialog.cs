using System;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.UIClasses;
using Domain;

namespace UI.Dialogs
{
    public class LearnDeckDialog : IDialog
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;
        private readonly string[] learnStates;

        private string deckId;
        private UICard learnCard;
        private State state = State.ChooseDeck;

        public LearnDeckDialog(CardApi cardApi, DeckApi deckApi)
        {
            this.cardApi = cardApi;
            this.deckApi = deckApi;

            learnStates = new[]
                {"🤡\nЗабыл", "😶\nсложно", "😜\nабоба", "👑\nИзи"};
        }

        public async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            var learnKeyboard = new[] { learnStates, new[] { "Закончил учить" } };

            if (state == State.ChooseDeck)
            {
                var decks = deckApi.GetDecksByUser(user);
                var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
                if (findDeck is null)
                {
                    await bot.SendMessage(user, "Выберите колоду:", false);
                    return this;
                }

                deckId = findDeck.Id;
                state = State.ViewFront;

                learnCard = cardApi.GetCardsToLearn(deckId).FirstOrDefault();
                if (learnCard is null)
                {
                    await bot.SendMessage(user, "Все карточки изучены, молодец!");
                    return null;
                }

                await bot.SendMessageWithKeyboard(user, learnCard.Front,
                    new KeyboardProvider(new[] { new[] { "Показать ответ" } }));
                return this;
            }

            if (state == State.ViewFront)
            {
                if (message == "Показать ответ")
                {
                    await bot.SendMessageWithKeyboard(user, learnCard.Back, new KeyboardProvider(learnKeyboard));
                    state = State.ViewBack;
                    return this;
                }

                await bot.SendMessageWithKeyboard(user, "Нажните \"Показать ответ\" когда будете готовы!",
                    new KeyboardProvider(new[] { new[] { "Показать ответ" } }));
                return this;
            }

            if (state == State.ViewBack)
            {
                if (message == "Закончил учить")
                {
                    await bot.SendMessage(user, "Возвращайся снова");
                    return null;
                }

                var learnState = learnStates.FirstOrDefault(s => s == message);
                if (learnState is null)
                {
                    await bot.SendMessageWithKeyboard(user, "Я жду ответа", new KeyboardProvider(learnKeyboard));
                    return this;
                }

                var answer = Array.FindIndex(learnStates, s => s == learnState);

                cardApi.LearnCard(learnCard.Id, answer);
                learnCard = cardApi.GetCardsToLearn(deckId).FirstOrDefault();
                if (learnCard is null)
                {
                    await bot.SendMessage(user, "Все карточки изучены, молодец!");
                    return null;
                }

                state = State.ViewFront;
                await bot.SendMessageWithKeyboard(user, learnCard.Front,
                    new KeyboardProvider(new[] { new[] { "Показать ответ" } }));
                return this;
            }

            return null;
        }

        private enum State
        {
            ChooseDeck,
            ViewFront,
            ViewBack
        }
    }
}