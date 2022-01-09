// using System.Linq;
// using System.Threading.Tasks;
// using App.APIs;
// using Domain;
//
// namespace UI.Commands.DeleteDeckCommands
// {
//     public class ChooseDeckCommand : ICommand
//     {
//         private readonly DeckApi deckApi;
//
//         public ChooseDeckCommand(DeckApi deckApi)
//         {
//             this.deckApi = deckApi;
//         }
//
//         public override string Name => "ChooseDeckForDeleteDeck";
//         public override bool IsInitial => false;
//
//         public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
//         {
//             var decksName = deckApi.GetDecksByUser(user);
//             var findDeck = decksName.FirstOrDefault(deck => deck.Name == message);
//             if (findDeck is null)
//             {
//                 await bot.SendMessage(user, "Выберите колоду:", false);
//                 return context;
//             }
//
//             deckApi.DeleteDeck(findDeck.Id);
//             await bot.SendMessage(user, "Колода успешно удалена!");
//             return new Context();
//         }
//     }
// }