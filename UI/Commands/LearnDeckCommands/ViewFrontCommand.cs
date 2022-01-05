using System.Threading.Tasks;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class ViewFrontCommand : Command
    {
        private readonly string[][] learnKeyboard =
        {
            new[] {"🤡\nЗабыл", "😶\nсложно", "😜\nабоба", "👑\nИзи"},
            new[] {"Закончил учить"}
        };

        public override string Name => "ViewFront";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            if (message == "Показать ответ")
            {
                await bot.SendMessageWithKeyboard(user, context.Back, new KeyboardProvider(learnKeyboard));
                return context;
            }

            await bot.SendMessageWithKeyboard(user, "Нажмите \"Показать ответ\" когда будете готовы!",
                new KeyboardProvider(new[] {new[] {"Показать ответ"}}));
            context.CommandName = "ViewBack";
            return context;
        }
    }
}