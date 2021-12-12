
using JsonKnownTypes;
using Newtonsoft.Json;

namespace AnkiBot.Domain.Parameters
{
    [JsonConverter(typeof(JsonKnownTypesConverter<IParameters>))]
    public interface IParameters
    {
    }
}