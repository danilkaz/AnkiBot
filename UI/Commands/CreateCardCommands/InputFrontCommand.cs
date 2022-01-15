using System.Threading.Tasks;
using Domain;

namespace UI.Commands.CreateCardCommands
{
    public class InputFrontCommand : ICommand
    {
        private readonly string[][] finishKeyboard = { new[] { "В главное меню" } };


        public InputFrontCommand(InputFrontData data)
        {
            Data = data;
        }

        private InputFrontData Data { get; }

        public string Name => "InputFront";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            if (message == finishKeyboard[0][0])
            {
                await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                return ICommandInfo.Create<StartCommand>();
            }

            var data = new InputBackData(Data.DeckId, message);
            await bot.SendMessageWithKeyboard(user, "Введите заднюю сторону карточки",
                new(finishKeyboard));
            return ICommandInfo.Create<InputBackData, InputBackCommand>(data);
        }
    }

    public class InputFrontCommandFactory : ICommandFactory<InputFrontData, InputFrontCommand>
    {
        public InputFrontCommand CreateCommand(InputFrontData data)
        {
            return new(data);
        }
    }

    public record InputFrontData(string DeckId) : IData;
}