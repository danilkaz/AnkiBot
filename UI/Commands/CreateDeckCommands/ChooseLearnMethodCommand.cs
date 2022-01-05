using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using Domain;
using Domain.LearnMethods;

namespace UI.Commands.CreateDeckCommands
{
    public class ChooseLearnMethodCommand : Command
    {
        private readonly DeckApi deckApi;
        private readonly ILearnMethod[] learnMethods;

        public ChooseLearnMethodCommand(ILearnMethod[] learnMethods, DeckApi deckApi)
        {
            this.learnMethods = learnMethods;
            this.deckApi = deckApi;
        }

        public override string Name => "ChooseLearnMethod";
        public override bool isInitial => false;

        public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
        {
            var keyboard = learnMethods.Select(m => new[] {m.Name}).Append(new[] {"Подробности"}).ToArray();
            if (message == "Подробности")
            {
                foreach (var method in learnMethods)
                    await bot.SendMessageWithKeyboard(user, method.Description, new KeyboardProvider(keyboard));
                return context;
            }

            var deckMethod = learnMethods.FirstOrDefault(m => m.Name.Equals(message));
            if (deckMethod is null)
            {
                await bot.SendMessageWithKeyboard(user, "Выберите метод", new KeyboardProvider(keyboard));
                return context;
            }

            deckApi.SaveDeck(user, context.DeckName, deckMethod, new List<Card>());
            await bot.SendMessage(user, "Колода успешно создана!");
            return new Context();
        }
    }
}