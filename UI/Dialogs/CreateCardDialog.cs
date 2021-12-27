using System.Linq;
using System.Threading.Tasks;
using App;
using App.UIClasses;
using Domain;

namespace UI.Dialogs
{
    public class CreateCardDialog : IDialog
    {
        private readonly CardApi cardApi;
        private readonly DeckApi deckApi;

        private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};
        private string back;
        private UIDeck deck;
        private string front;
        private State state = State.ChooseDeck;

        public CreateCardDialog(DeckApi deckApi, CardApi cardApi)
        {
            this.deckApi = deckApi;
            this.cardApi = cardApi;
        }

        public async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            if (message == finishKeyboard[0][0])
                return null;
            switch (state)
            {
                case State.ChooseDeck:
                {
                    var decks = deckApi.GetDecksByUser(user);
                    deck = decks.FirstOrDefault(d => d.Name == message);
                    if (deck is null)
                    {
                        await bot.SendMessage(user, "Выберите колоду:", false);
                        return this;
                    }

                    state = State.InputFront;
                    await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
                        new KeyboardProvider(finishKeyboard));
                    return this;
                }
                case State.InputFront:
                {
                    front = message;
                    state = State.InputBack;
                    await bot.SendMessageWithKeyboard(user, "Введите заднюю сторону карточки",
                        new KeyboardProvider(finishKeyboard));
                    return this;
                }
                case State.InputBack:
                {
                    back = message;
                    cardApi.SaveCard(user, deck.Id, front, back);
                    state = State.InputFront;
                    await bot.SendMessage(user, "Карточка успешно сохранена");
                    await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
                        new KeyboardProvider(finishKeyboard));
                    return this;
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