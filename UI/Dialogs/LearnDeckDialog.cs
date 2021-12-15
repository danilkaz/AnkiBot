using System;
using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.UI.Commands;

namespace UI.Dialogs
{
    public class LearnDeckDialog : IDialog
    {
        private readonly string[] learnStates;
        private readonly IRepository repository;

        private string deckId;
        private Card learnCard;
        private ILearnMethod learnMethod;
        private State state = State.ChooseDeck;

        public LearnDeckDialog(IRepository repository)
        {
            this.repository = repository;

            learnStates = new[]
                {"🤡\nЗабыл", "😶\nсложно", "😜\nабоба", "👑\nИзи"};
        }

        public async Task<IDialog> Execute(User user, string message, Bot bot)
        {
            var learnKeyboard = new[] {learnStates, new[] {"Закончил учить"}};

            if (state == State.ChooseDeck)
            {
                var decks = repository.GetDecksByUser(user);
                var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
                if (findDeck is null)
                {
                    await bot.SendMessage(user, "Выберите колоду:", false);
                    return this;
                }

                learnMethod = findDeck.LearnMethod;
                deckId = findDeck.Id.ToString();
                state = State.ViewFront;

                learnCard = repository.GetCardsToLearn(deckId).FirstOrDefault();
                if (learnCard is null)
                {
                    await bot.SendMessage(user, "Все карточки изучены, молодец!");
                    return null;
                }

                await bot.SendMessageWithKeyboard(user, learnCard.Front, new[] {new[] {"Показать ответ"}});
                return this;
            }

            if (state == State.ViewFront)
            {
                if (message == "Показать ответ")
                {
                    await bot.SendMessageWithKeyboard(user, learnCard.Back, learnKeyboard);
                    state = State.ViewBack;
                    return this;
                }

                await bot.SendMessageWithKeyboard(user, "Нажните \"Показать ответ\" когда будете готовы!",
                    new[] {new[] {"Показать ответ"}});
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
                    await bot.SendMessageWithKeyboard(user, "Я жду ответа", learnKeyboard);
                    return this;
                }

                var answer = Array.FindIndex(learnStates, s => s == learnState) + 2;

                learnCard.TimeBeforeLearn = learnMethod.GetNextRepetition(learnCard, answer);
                learnCard.LastLearnTime = DateTime.Now;

                repository.UpdateCard(learnCard);
                learnCard = repository.GetCardsToLearn(deckId).FirstOrDefault();
                if (learnCard is null)
                {
                    await bot.SendMessage(user, "Все карточки изучены, молодец!");
                    return null;
                }

                state = State.ViewFront;
                await bot.SendMessageWithKeyboard(user, learnCard.Front, new[] {new[] {"Показать ответ"}});
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