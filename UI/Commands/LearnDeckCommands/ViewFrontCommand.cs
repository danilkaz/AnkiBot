using System.Threading.Tasks;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class ViewFrontCommand : ICommand
    {
        private readonly string[][] learnKeyboard =
        {
            new[] {"🤡\nЗабыл", "😶\nсложно", "😜\nабоба", "👑\nИзи"},
            new[] {"Закончил учить"}
        };

        public ViewFrontCommand(ViewFrontData data)
        {
            Data = data;
        }

        private ViewFrontData Data { get; }

        public string Name => "ViewFront";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            if (message != "Показать ответ")
            {
                await bot.SendMessageWithKeyboard(user, "Нажмите \"Показать ответ\" когда будете готовы!",
                    new(new[] { new[] { "Показать ответ" } }));
                return ICommandInfo.Create<ViewFrontData, ViewFrontCommand>(Data);
            }

            await bot.SendMessageWithKeyboard(user, Data.Back, new(learnKeyboard));
            var data = new ViewBackData(Data.DeckId, Data.CardId, Data.Front, Data.Front);
            return ICommandInfo.Create<ViewBackData, ViewBackCommand>(data);
        }
    }

    public class ViewFrontCommandFactory : ICommandFactory<ViewFrontData, ViewFrontCommand>
    {
        public ViewFrontCommand CreateCommand(ViewFrontData data)
        {
            return new(data);
        }
    }

    public record ViewFrontData(string DeckId, string CardId, string Front, string Back) : IData;
}