// using System.Linq;
// using System.Threading.Tasks;
// using App.APIs;
// using Domain;
// using Domain.LearnMethods;
//
// namespace UI.Commands.CreateDeckCommands
// {
//     public class InputDeckNameCommand : ICommand
//     {
//         private readonly DeckApi deckApi;
//         private readonly ILearnMethod[] learnMethods;
//
//         public InputDeckNameCommand(DeckApi deckApi, ILearnMethod[] learnMethods)
//         {
//             this.deckApi = deckApi;
//             this.learnMethods = learnMethods;
//         }
//
//         public override string Name => "InputDeckName";
//         public override bool IsInitial => false;
//
//         public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
//         {
//             var deck = deckApi.GetDecksByUser(user).FirstOrDefault(d => d.Name == message);
//
//             if (deck != null)
//             {
//                 await bot.SendMessage(user, "Колода с таким именем уже создана");
//                 await bot.SendMessage(user, "Введите имя колоды");
//                 return context;
//             }
//
//             context.DeckName = message;
//             var keyboard = learnMethods.Select(m => new[] {m.Name}).Append(new[] {"Подробности"}).ToArray();
//             await bot.SendMessageWithKeyboard(user, "Выберите метод для запоминания",
//                 new KeyboardProvider(keyboard));
//             context.CommandName = "ChooseLearnMethod";
//             return context;
//         }
//     }
// }