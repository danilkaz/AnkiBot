using Newtonsoft.Json;
using UI.JsonKnownTypes;

namespace UI.Commands
{
    [JsonConverter(typeof(MyJsonKnownTypesConverter<IData>))]
    public interface IData
    {
    }
}