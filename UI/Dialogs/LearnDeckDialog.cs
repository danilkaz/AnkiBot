using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.Domain.LearnMethods;
using AnkiBot.UI.Commands;
using Infrastructure.Attributes;

namespace UI.Dialogs
{
    public class LearnDeckDialog : IDialog
    {
        private State state = State.ChooseDeck;
        private readonly IRepository repository;
        private ILearnMethod learnMethod;
        
        private string deckId;
        private Card learnCard;

        private string[] learnStates;

        public LearnDeckDialog(IRepository repository)
        {
            this.repository = repository;

            learnStates = new[] {"🤡\nЗабыл", "😶\nвавкнвы", "😜\nавава", "👑\nИзи"};
        }

        public async Task<IDialog> Execute(long userId, string message, Bot bot)
        {
            var learnKeyboard = new[] {learnStates, new[] {"Закончил учить"}};

            if (state == State.ChooseDeck)
            {
                var decks = repository.GetDecksByUserId(userId.ToString());
                var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
                if (findDeck is null)
                {
                    await bot.SendMessage(userId, "Выберите колоду:", false);
                    return this;
                }

                learnMethod = findDeck.LearnMethod;
                deckId = findDeck.Id.ToString();
                state = State.ViewFront;

                learnCard = repository.GetCardsToLearn(deckId).FirstOrDefault();
                if (learnCard is null)
                {
                    await bot.SendMessage(userId, "Все карточки изучены, молодец!");
                    return null;
                }
                await bot.SendMessageWithKeyboard(userId, learnCard.Front, new[] {new[] {"Показать ответ"}});
                return this;
            }

            if (state == State.ViewFront)
            {
                if (message == "Показать ответ")
                {
                    await bot.SendMessageWithKeyboard(userId, learnCard.Back, learnKeyboard);
                    state = State.ViewBack;
                    return this;
                }

                await bot.SendMessageWithKeyboard(userId, "Нажните \"Показать ответ\" когда будете готовы!",
                    new[] {new[] {"Показать ответ"}});
                return this;
            }

            if (state == State.ViewBack)
            {
                if (message == "Закончил учить")
                {
                    await bot.SendMessage(userId, "Возвращайся снова");
                    return null;
                }

                var learnState = learnStates.FirstOrDefault(s => s == message);
                if (learnState is null)
                {
                    await bot.SendMessageWithKeyboard(userId, "Я жду ответа", learnKeyboard);
                    return this;
                }

                var answer = Array.FindIndex(learnStates, s => s == learnState);
                
                learnCard.TimeBeforeLearn = learnMethod.GetNextRepetition(learnCard, answer);
                learnCard.LastLearnTime = DateTime.Now;
                
                repository.UpdateCard(learnCard);
                learnCard = repository.GetCardsToLearn(deckId).FirstOrDefault();
                Console.WriteLine(learnCard.NextLearnTime);
                if (learnCard is null)
                {
                    await bot.SendMessage(userId, "Все карточки изучены, молодец!");
                    return null;
                }
                state = State.ViewFront;
                await bot.SendMessageWithKeyboard(userId, learnCard.Front, new[] {new[] {"Показать ответ"}});
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