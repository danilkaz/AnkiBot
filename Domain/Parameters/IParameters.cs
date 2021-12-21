using Domain.Visitors;
using JsonKnownTypes;
using Newtonsoft.Json;

namespace Domain.Parameters
{
    [JsonConverter(typeof(JsonKnownTypesConverter<IParameters>))]
    public interface IParameters
    {
        void Accept(IVisitor visitor);
    }
}