using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.CreateCardCommands
{
    public class InputBackCommand : ICommand
    {
        private readonly CardApi cardApi;
        private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};

        public InputBackCommand(InputBackData data, CardApi cardApi)
        {
            Data = data;
            this.cardApi = cardApi;
        }

        private InputBackData Data { get; }


        public string Name => "InputBack";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            if (message == finishKeyboard[0][0])
            {
                await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                return ICommandInfo.Create<StartCommand>();
            }

            cardApi.SaveCard(user, Data.DeckId, Data.Front, message);
            await bot.SendMessage(user, "Карточка успешно сохранена");
            await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
                new(finishKeyboard));
            return ICommandInfo.Create<InputFrontData, InputFrontCommand>(new(Data.DeckId));
        }
    }

    public class InputBackCommandFactory : ICommandFactory<InputBackData, InputBackCommand>
    {
        private readonly CardApi cardApi;

        public InputBackCommandFactory(CardApi cardApi)
        {
            this.cardApi = cardApi;
        }

        public InputBackCommand CreateCommand(InputBackData data)
        {
            return new(data, cardApi);
        }
    }

    public record InputBackData(string DeckId, string Front) : IData;
}