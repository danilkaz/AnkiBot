// using System.Linq;
// using System.Threading.Tasks;
// using App.APIs;
// using Domain;
// using UI.Data;
//
// namespace UI.Commands.CreateCardCommands
// {
//     public class InitialCreateCardCommand : ICommand<object>
//     {
//         private readonly DeckApi deckApi;
//         private readonly ChooseDeckCommandFactory chooseDeckCommandFactory;
//
//         public InitialCreateCardCommand(DeckApi deckApi, ChooseDeckCommandFactory chooseDeckCommandFactory)
//         {
//             this.deckApi = deckApi;
//             this.chooseDeckCommandFactory = chooseDeckCommandFactory;
//         }
//
//         public string Name => "Добавить карточку";
//         public bool IsInitial => true;
//
//         public async Task<ICommand<object>> Execute(User user, string message, IBot bot)
//         {
//             var decksNames = deckApi.GetDecksByUser(user);
//             if (!decksNames.Any())
//             {
//                 await bot.SendMessage(user, "У вас нет ни одной колоды. Сначала создайте ее", false);
//                 return null;
//             }
//
//             var decksKeyboard = decksNames
//                 .Select(d => new[] {d.Name})
//                 .ToArray();
//             await bot.SendMessageWithKeyboard(user, "Выберите колоду:", new KeyboardProvider(decksKeyboard));
//             return chooseDeckCommandFactory.CreateCommand(new ChooseDeckData());
//         }
//
//         public object Data { get; set; }
//     }
// }