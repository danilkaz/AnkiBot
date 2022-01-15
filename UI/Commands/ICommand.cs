using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using Ninject;
using UI.JsonKnownTypes;

namespace UI.Commands
{
    public interface ICommand
    {
        public string Name { get; }
        public bool IsInitial { get; }
        public Task<ICommandInfo> Execute(User user, string message, IBot bot);
    }

    [JsonConverter(typeof(MyJsonKnownTypesConverter<ICommandInfo>))]
    public interface ICommandInfo
    {
        public static ICommandInfo Create<TCom>() where TCom : ICommand
        {
            return new CommandInfoWithoutData<TCom>();
        }

        public static ICommandInfo Create<TData, TCom>(TData data) where TCom : ICommand
        {
            return new CommandInfoWithData<TData, TCom>(data);
        }

        public ICommand GetCommand(StandardKernel container);
    }

    public class CommandInfoWithData<TData, TCom> : ICommandInfo where TCom : ICommand
    {
        [JsonProperty] private readonly TData data;

        public CommandInfoWithData(TData data)
        {
            this.data = data;
        }

        public ICommand GetCommand(StandardKernel container)
        {
            var commandFactory = container.Get<ICommandFactory<TData, TCom>>();
            return commandFactory.CreateCommand(data);
        }
    }

    public class CommandInfoWithoutData<TCom> : ICommandInfo where TCom : ICommand
    {
        public ICommand GetCommand(StandardKernel container)
        {
            return container.Get<TCom>();
        }
    }
}