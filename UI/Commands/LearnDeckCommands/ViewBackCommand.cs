using System;
using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class ViewBackCommand : ICommand
    {
        private readonly CardApi cardApi;
        private readonly string[][] learnKeyboard;
        private readonly string[] learnStates = { "🤡\nСнова", "😶\nТрудно", "😜\nХорошо", "👑\nЛегко" };

        public ViewBackCommand(ViewBackData data, CardApi cardApi)
        {
            Data = data;
            this.cardApi = cardApi;
            learnKeyboard = new[] { learnStates, new[] { "Закончил учить" } };
        }

        private ViewBackData Data { get; }

        public string Name => "ViewBack";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            if (message == "Закончил учить")
            {
                await bot.SendMessage(user, "Возвращайся снова");
                return null;
            }

            var learnState = learnStates.FirstOrDefault(s => s == message);
            if (learnState is null)
            {
                await bot.SendMessageWithKeyboard(user, "Я жду ответа", new(learnKeyboard));
                return ICommandInfo.Create<ViewBackData, ViewBackCommand>(Data);
            }

            var answer = Array.FindIndex(learnStates, s => s == learnState);

            cardApi.LearnCard(Data.CardId, answer);

            var learnCard = cardApi.GetCardsToLearn(Data.DeckId).FirstOrDefault();
            if (learnCard is null)
            {
                await bot.SendMessage(user, "Все карточки изучены, молодец!");
                await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
                return ICommandInfo.Create<StartCommand>();
            }

            var data = new ViewFrontData(Data.DeckId, learnCard.Id, learnCard.Front, learnCard.Back);

            await bot.SendMessageWithKeyboard(user, learnCard.Front, new(new[] { new[] { "Показать ответ" } }));
            return ICommandInfo.Create<ViewFrontData, ViewFrontCommand>(data);
        }
    }

    public class ViewBackCommandFactory : ICommandFactory<ViewBackData, ViewBackCommand>
    {
        private readonly CardApi cardApi;

        public ViewBackCommandFactory(CardApi cardApi)
        {
            this.cardApi = cardApi;
        }

        public ViewBackCommand CreateCommand(ViewBackData data)
        {
            return new(data, cardApi);
        }
    }

    public record ViewBackData(string DeckId, string CardId, string Front, string Back) : IData;
}