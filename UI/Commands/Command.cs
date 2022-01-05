using System.Threading.Tasks;
using Domain;
using JsonKnownTypes;
using Newtonsoft.Json;

namespace UI.Commands
{
    [JsonConverter(typeof(JsonKnownTypesConverter<Command>))]
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract bool isInitial { get; }
        public abstract Task<Context> Execute(User user, string message, IBot bot, Context context);
    }
}