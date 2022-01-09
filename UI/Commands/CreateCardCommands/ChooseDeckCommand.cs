// using System.Linq;
// using System.Threading.Tasks;
// using App.APIs;
// using Domain;
// using UI.Data;
//
// namespace UI.Commands.CreateCardCommands
// {
//     public class ChooseDeckCommand : ICommand
//     {
//         private readonly DeckApi deckApi;
//         private readonly InputFrontCommandFactory inputFrontCommandFactory;
//         private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};
//
//         public ChooseDeckCommand(DeckApi deckApi, InputFrontCommandFactory inputFrontCommandFactory)
//         {
//             this.deckApi = deckApi;
//             this.inputFrontCommandFactory = inputFrontCommandFactory;
//         }
//
//         public string Name => "ChooseDeckForCreateCard";
//         public bool IsInitial => false;
//
//         public async Task<INext<object>> Execute(User user, string message, IBot bot)
//         {
//             var decks = deckApi.GetDecksByUser(user);
//             var deck = decks.FirstOrDefault(d => d.Name == message);
//             if (deck is null)
//             {
//                 await bot.SendMessage(user, "Выберите колоду:", false);
//                 return this;
//             }
//
//             var data = new InputFrontData(deck.Id);
//
//             await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
//                 new KeyboardProvider(finishKeyboard));
//             return inputFrontCommandFactory.CreateCommand(data);
//         }
//
//         public ChooseDeckData Data { get; set; }
//     }
//
//     public class ChooseDeckCommandFactory : ICommandFactory<ChooseDeckData, ChooseDeckCommand>
//     {
//         private readonly DeckApi deckApi;
//         private readonly InputFrontCommandFactory inputFrontCommandFactory;
//
//         public ChooseDeckCommandFactory(DeckApi deckApi, InputFrontCommandFactory inputFrontCommandFactory)
//         {
//             this.deckApi = deckApi;
//             this.inputFrontCommandFactory = inputFrontCommandFactory;
//         }
//
//         public ChooseDeckCommand CreateCommand(ChooseDeckData data)
//         {
//             return new(deckApi, inputFrontCommandFactory);
//         }
//     }
//
//     public record ChooseDeckData();
// }