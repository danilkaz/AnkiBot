// using System.Linq;
// using System.Threading.Tasks;
// using App.APIs;
// using Domain;
//
// namespace UI.Commands.DeleteCardCommands
// {
//     public class ChooseCardCommand : ICommand
//     {
//         private readonly CardApi cardApi;
//
//         public ChooseCardCommand(CardApi cardApi)
//         {
//             this.cardApi = cardApi;
//         }
//
//         public override string Name => "ChooseCard";
//         public override bool IsInitial => false;
//
//         public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
//         {
//             var splitMessage = message.Split('\n');
//             var card = cardApi.GetCardsByDeckId(context.DeckId)
//                 .FirstOrDefault(c => c.Id.ToString() == splitMessage.Last());
//             if (card is null)
//             {
//                 await bot.SendMessage(user, "Выберите карту:", false);
//                 return context;
//             }
//
//             cardApi.DeleteCard(card.Id);
//             await bot.SendMessage(user, "Карта успешно удалена", false);
//             return new Context();
//         }
//     }
// }