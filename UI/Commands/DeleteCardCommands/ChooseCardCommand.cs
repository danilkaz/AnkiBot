using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;

namespace UI.Commands.DeleteCardCommands
{
    public class ChooseCardCommand : ICommand
    {
        private readonly CardApi cardApi;

        public ChooseCardCommand(ChooseCardData data, CardApi cardApi)
        {
            Data = data;
            this.cardApi = cardApi;
        }

        private ChooseCardData Data { get; }

        public string Name => "ChooseCard";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var splitMessage = message.Split('\n');
            var card = cardApi.GetCardsByDeckId(Data.DeckId)
                .FirstOrDefault(c => c.Id.ToString() == splitMessage.Last());
            if (card is null)
            {
                await bot.SendMessage(user, "Выберите карту:", false);
                return ICommandInfo.Create<ChooseCardData, ChooseCardCommand>(Data);
            }

            cardApi.DeleteCard(card.Id);
            await bot.SendMessage(user, "Карта успешно удалена", false);
            await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
            return ICommandInfo.Create<StartCommand>();
        }
    }

    public class ChooseCardCommandFactory : ICommandFactory<ChooseCardData, ChooseCardCommand>
    {
        private readonly CardApi cardApi;

        public ChooseCardCommandFactory(CardApi cardApi)
        {
            this.cardApi = cardApi;
        }

        public ChooseCardCommand CreateCommand(ChooseCardData data)
        {
            return new(data, cardApi);
        }
    }

    public record ChooseCardData(string DeckId) : IData;
}