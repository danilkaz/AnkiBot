using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.APIs;
using Domain;
using Domain.LearnMethods;

namespace UI.Commands.CreateDeckCommands
{
    public class ChooseLearnMethodCommand : ICommand
    {
        private readonly DeckApi deckApi;
        private readonly ILearnMethod[] learnMethods;

        public ChooseLearnMethodCommand(ChooseLearnMethodData data, DeckApi deckApi, ILearnMethod[] learnMethods)
        {
            Data = data;
            this.learnMethods = learnMethods;
            this.deckApi = deckApi;
        }

        private ChooseLearnMethodData Data { get; }

        public string Name => "ChooseLearnMethod";
        public bool IsInitial => false;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            var keyboard = learnMethods.Select(m => new[] {m.Name}).Append(new[] {"Подробности"}).ToArray();
            if (message == "Подробности")
            {
                foreach (var method in learnMethods)
                    await bot.SendMessageWithKeyboard(user, method.Description, new(keyboard));
                return ICommandInfo.Create<ChooseLearnMethodData, ChooseLearnMethodCommand>(Data);
            }

            var deckMethod = learnMethods.FirstOrDefault(m => m.Name.Equals(message));
            if (deckMethod is null)
            {
                await bot.SendMessageWithKeyboard(user, "Выберите метод", new(keyboard));
                return ICommandInfo.Create<ChooseLearnMethodData, ChooseLearnMethodCommand>(Data);
            }

            deckApi.SaveDeck(user, Data.DeckName, deckMethod, new List<Card>());
            await bot.SendMessage(user, "Колода успешно создана!");
            await bot.SendMessageWithKeyboard(user, "Вот что я умею:", KeyboardProvider.DefaultKeyboard);
            return ICommandInfo.Create<StartCommand>();
        }
    }

    public class ChooseLearnMethodCommandFactory : ICommandFactory<ChooseLearnMethodData, ChooseLearnMethodCommand>
    {
        private readonly DeckApi deckApi;
        private readonly ILearnMethod[] learnMethods;

        public ChooseLearnMethodCommandFactory(DeckApi deckApi, ILearnMethod[] learnMethods)
        {
            this.deckApi = deckApi;
            this.learnMethods = learnMethods;
        }

        public ChooseLearnMethodCommand CreateCommand(ChooseLearnMethodData data)
        {
            return new(data, deckApi, learnMethods);
        }
    }

    public record ChooseLearnMethodData(string DeckName) : IData;
}