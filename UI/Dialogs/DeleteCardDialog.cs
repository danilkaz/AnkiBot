using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using AnkiBot.UI.Commands;

namespace UI.Dialogs
{
    public class DeleteCardDialog : IDialog
    {
        private State state = State.ChooseDeck;
        private readonly IRepository repository;

        private string deckId;
        private IEnumerable<Card> cards; 

        public DeleteCardDialog(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDialog> Execute(long userId, string message, Bot bot)
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
                    cards = repository.GetCardsByDeckId(deckId);
                    if (!cards.Any())
                    {
                        await bot.SendMessage(userId, "Колода пуста", false);
                        return null;
                    }
                    var cardsKeyboard = cards.Select(c => new[] {c.Front + "\n" + c.Id});
                    state = State.ChooseCard;
                    await bot.SendMessageWithKeyboard(userId, "Выберите карту:", cardsKeyboard);
                    return this;
                }
                case State.ChooseCard:
                {
                    var splitMessage = message.Split('\n');
                    var card = cards.FirstOrDefault(c => c.Id.ToString() == splitMessage.Last());
                    if (card is null)
                    {
                        await bot.SendMessage(userId, "Выберите карту:", false);
                        return this;
                    }
                    repository.DeleteCard(card.Id.ToString());
                    await bot.SendMessage(userId, "Карта успешно удалена", false);
                    return null;
                }
                default:
                    return null;
            }
        }

        private enum State
        {
            ChooseDeck,
            ChooseCard
        }
    }
}