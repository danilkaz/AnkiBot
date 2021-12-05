using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.UI.Commands;
using VkNet.Model;

namespace UI.Dialogs
{
    public class CreateCardDialog : IDialog
    {
        private State state = State.ChooseDeck;
        private readonly IRepository repository;

        private string deckId;
        private string front;
        private string back;

        public CreateCardDialog(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IDialog> Execute(long userId, string message, IBot bot)
        {
            switch (state)
            {
                case State.ChooseDeck:
                {
                    var decks = repository.GetDecksByUserId(userId.ToString());
                    var findDeck = decks.FirstOrDefault(deck => deck.Name == message);
                    if (findDeck is null)
                    {
                        await bot.SendMessage(userId, "Выберите колоду:", false);
                        return this;
                    }
                    deckId = findDeck.Id.ToString();
                    state = State.InputFront;
                    await bot.SendMessage(userId, "Введите переднюю сторону карточки");
                    return this;
                }
                case State.InputFront:
                {
                    front = message;
                    state = State.InputBack;
                    await bot.SendMessage(userId, "Введите заднюю сторону карточки");
                    return this;
                }
                case State.InputBack:
                {
                    back = message;
                    var card = new Card(userId.ToString(), deckId, front, back);
                    repository.SaveCard(card);
                    await bot.SendMessage(userId, "Карточка успешно сохранена");
                    Console.WriteLine(repository.GetCard(card.Id.ToString()).Front);
                    return null;
                }
                default: return null;
            }
        }
        
        private enum State
        {
            ChooseDeck,
            InputFront,
            InputBack
        }
    }
}