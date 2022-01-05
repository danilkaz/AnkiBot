using System;
using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;

namespace UI.Commands.LearnDeckCommands
{
    public class ViewBackCommand : Command
    {
        private readonly CardApi cardApi;
        private readonly string[][] learnKeyboard;
        private readonly string[] learnStates = {"🤡\nЗабыл", "😶\nсложно", "😜\nабоба", "👑\nИзи"};

        public ViewBackCommand(CardApi cardApi)
        {
            this.cardApi = cardApi;
            learnKeyboard = new[] {learnStates, new[] {"Закончил учить"}};
        }

        public override string Name => "ViewBack";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            if (message == "Закончил учить")
            {
                await bot.SendMessage(user, "Возвращайся снова");
                return null;
            }

            var learnState = learnStates.FirstOrDefault(s => s == message);
            if (learnState is null)
            {
                await bot.SendMessageWithKeyboard(user, "Я жду ответа", new KeyboardProvider(learnKeyboard));
                return context;
            }

            var answer = Array.FindIndex(learnStates, s => s == learnState);

            cardApi.LearnCard(context.CardId, answer);

            var learnCard = cardApi.GetCardsToLearn(context.DeckId).FirstOrDefault();
            if (learnCard is null)
            {
                await bot.SendMessage(user, "Все карточки изучены, молодец!");
                return new Context();
            }

            context.CardId = learnCard.Id;
            context.Front = learnCard.Front;
            context.Back = learnCard.Back;

            await bot.SendMessageWithKeyboard(user, learnCard.Front,
                new KeyboardProvider(new[] {new[] {"Показать ответ"}}));
            return context;
        }
    }
}