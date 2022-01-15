using System.Threading.Tasks;
using Domain;

namespace UI.Commands.CreateDeckCommands
{
    public class InitialCreateDeckCommand : ICommand
    {
        public string Name => "Создать колоду";
        public bool IsInitial => true;

        public async Task<ICommandInfo> Execute(User user, string message, IBot bot)
        {
            await bot.SendMessage(user, "Введите имя колоды");
            return ICommandInfo.Create<InputDeckNameCommand>();
        }
    }
}