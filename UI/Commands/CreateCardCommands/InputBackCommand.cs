// using System;
// using System.Threading.Tasks;
// using App.APIs;
// using Domain;
// using UI.Data;
//
// namespace UI.Commands.CreateCardCommands
// {
//     public class InputBackCommand : ICommand<InputBackData>
//     {
//         private readonly CardApi cardApi;
//         private readonly StartCommandFactory startCommandFactory;
//         private readonly InputFrontCommandFactory inputFrontCommandFactory;
//         private readonly string[][] finishKeyboard = {new[] {"В главное меню"}};
//
//         public InputBackCommand(InputBackData data, CardApi cardApi, StartCommandFactory startCommandFactory,
//             InputFrontCommandFactory inputFrontCommandFactory)
//         {
//             Data = data;
//             this.cardApi = cardApi;
//             this.startCommandFactory = startCommandFactory;
//             this.inputFrontCommandFactory = inputFrontCommandFactory;
//         }
//
//         public string Name => "InputBack";
//         public bool IsInitial => false;
//
//         public async Task<ICommand<object>> Execute(User user, string message, IBot bot)
//         {
//             if (message == finishKeyboard[0][0])
//                 return startCommandFactory.CreateCommand(new EmptyData());
//             cardApi.SaveCard(user, Data.DeckId, Data.Front, message);
//             await bot.SendMessage(user, "Карточка успешно сохранена");
//             await bot.SendMessageWithKeyboard(user, "Введите переднюю сторону карточки",
//                 new KeyboardProvider(finishKeyboard));
//             return inputFrontCommandFactory.CreateCommand(new InputFrontData(Data.DeckId));
//         }
//
//         public InputBackData Data { get; set; }
//     }
//
//     public class InputBackCommandFactory : ICommandFactory<InputBackData, InputBackCommand>
//     {
//         private readonly CardApi cardApi;
//         private readonly StartCommandFactory startCommandFactory;
//         private readonly Lazy<InputFrontCommandFactory> inputFrontCommandFactory;
//
//
//         public InputBackCommandFactory(CardApi cardApi, StartCommandFactory startCommandFactory, Lazy<InputFrontCommandFactory> inputFrontCommandFactory)
//         {
//             this.cardApi = cardApi;
//             this.startCommandFactory = startCommandFactory;
//             this.inputFrontCommandFactory = inputFrontCommandFactory;
//         }
//
//         public InputBackCommand CreateCommand(InputBackData data)
//         {
//             return new(data, cardApi, startCommandFactory, inputFrontCommandFactory.Value);
//         }
//     }
//
//     public record InputBackData(string DeckId, string Front);
// }