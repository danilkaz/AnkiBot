// using System.Threading.Tasks;
// using App.APIs;
// using Domain;
// using Domain.LearnMethods;
//
// namespace UI.Commands.CreateDeckCommands
// {
//     public class InitialCreateDeckCommand : ICommand
//     {
//         private readonly DeckApi deckApi;
//         private readonly ILearnMethod[] learnMethods;
//
//         public InitialCreateDeckCommand(ILearnMethod[] learnMethods, DeckApi deckApi)
//         {
//             this.learnMethods = learnMethods;
//             this.deckApi = deckApi;
//         }
//
//         public override string Name => "Создать колоду";
//         public override bool IsInitial => true;
//
//         public override async Task<Context> Execute(User user, string message, IBot bot, Context context)
//         {
//             await bot.SendMessage(user, "Введите имя колоды");
//             context.CommandName = "InputDeckName";
//             return context;
//         }
//     }
// }