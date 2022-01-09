// using System.Threading.Tasks;
// using Domain;
// using Newtonsoft.Json.Serialization;
// using UI.Data;
//
// namespace UI.Commands.CreateCardCommands
// {
//     public class InputFrontCommand : ICommand<InputFrontData>
//     {
//         private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};
//
//         private readonly InputBackCommandFactory inputBackCommandFactory;
//         private readonly StartCommandFactory startCommandFactory;
//
//         public InputFrontCommand(InputFrontData data, InputBackCommandFactory inputBackCommandFactory,
//             StartCommandFactory startCommandFactory)
//         {
//             Data = data;
//             this.inputBackCommandFactory = inputBackCommandFactory;
//             this.startCommandFactory = startCommandFactory;
//         }
//
//         public string Name => "InputFront";
//         public bool IsInitial => false;
//
//         public async Task<ICommand<object>> Execute(User user, string message, IBot bot)
//         {
//             if (message == finishKeyboard[0][0])
//                 return startCommandFactory.CreateCommand(new EmptyData());
//             var data = new InputBackData(Data.DeckId, message);
//             await bot.SendMessageWithKeyboard(user, "Введите заднюю сторону карточки",
//                 new KeyboardProvider(finishKeyboard));
//             return inputBackCommandFactory.CreateCommand(data);
//         }
//
//         public InputFrontData Data { get; set; }
//     }
//
//     public class InputFrontCommandFactory : ICommandFactory<InputFrontData, InputFrontCommand>
//     {
//         private readonly InputBackCommandFactory inputBackCommandFactory;
//         private readonly StartCommandFactory startCommandFactory;
//
//         public InputFrontCommandFactory(InputBackCommandFactory inputBackCommandFactory, StartCommandFactory startCommandFactory)
//         {
//             this.inputBackCommandFactory = inputBackCommandFactory;
//             this.startCommandFactory = startCommandFactory;
//         }
//
//         public InputFrontCommand CreateCommand(InputFrontData data)
//         {
//             return new InputFrontCommand(data, inputBackCommandFactory, startCommandFactory);
//         }
//     }
//
//     public record InputFrontData(string DeckId);
// }