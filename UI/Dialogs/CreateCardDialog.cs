using System.Linq;
using System.Threading.Tasks;
using AnkiBot.App;
using AnkiBot.Domain;
using App;

namespace UI.Dialogs
{
    public class CreateCardDialog : IDialog
    {
        
        private readonly IRepository repository;
        private readonly Converter converter;
        
        private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};
        private Deck deck;
        private string front;
        private string back;
        private State state = State.ChooseDeck;

        public CreateCardDialog(IRepository repository, Converter converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        public async Task<IDialog> Execute(User user, string message, IBot bot)
        {
            if (message == finishKeyboard[0][0])
                return null;
            switch (state)
            {
                case State.ChooseDeck:
                {
                    var decks = repository.GetDecksByUser(user).Select(converter.ToDeck);
                    var findDeck = decks.FirstOrDefault(d => d.Name == message);
                    if (findDeck is null)
                    {
                        await bot.SendMessage(user, "Выберите колоду:", false);
                        return this;
                    }

                    deck = findDeck;
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
                    var card = new Card(user, deck.Id, front, back,
                        deck.LearnMethod.GetParameters());
                    repository.SaveCard(card);

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