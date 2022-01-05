using System.Threading.Tasks;
using Domain;

namespace UI.Commands.CreateCardCommands
{
    public class InputFrontCommand : Command
    {
        private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};
        public override string Name => "InputFront";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            if (message == finishKeyboard[0][0])
                return new Context();
            context.Front = message;
            context.CommandName = "InputBack";
            await bot.SendMessageWithKeyboard(user, "Введите заднюю сторону карточки",
                new KeyboardProvider(finishKeyboard));
            return context;
        }
    }
}