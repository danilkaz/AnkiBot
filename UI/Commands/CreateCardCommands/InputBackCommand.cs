using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.CreateCardCommands
{
    public class InputBackCommand : Command
    {
        private readonly CardApi cardApi;
        private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};

        public InputBackCommand(CardApi cardApi)
        {
            this.cardApi = cardApi;
        }

        public override string Name => "InputBack";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            if (message == finishKeyboard[0][0])
                return new Context();
            context.Back = message;
            context.CommandName = "InputFront";
            cardApi.SaveCard(user, context.DeckId, context.Front, context.Back);
            await bot.SendMessage(user, "Карточка успешно сохранена");
            await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
                new KeyboardProvider(finishKeyboard));
            return context;
        }
    }
}