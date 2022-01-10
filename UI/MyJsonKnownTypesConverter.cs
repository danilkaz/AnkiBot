using System;
using System.Threading;
using System.Threading.Tasks;
using JsonKnownTypes.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UI.JsonKnownTypes
{
    public class MyJsonKnownTypesConverter<T> : JsonConverter
    {
        private readonly ThreadLocal<bool> isInRead = new();

        private readonly ThreadLocal<bool> isInWrite = new();

        public override bool CanRead => !IsInReadAndReset();

        public override bool CanWrite => !IsInWriteAndReset();

        public override bool CanConvert(Type objectType)
        {
            return isInRead.Value != true;
        }

        private bool IsInReadAndReset()
        {
            if (!isInRead.Value) return false;
            isInRead.Value = false;
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var jObject = JObject.Load(reader);

            var discriminator = jObject["type"]?.Value<string>();

            if (TypesDiscriminatorValues.TryGetType(discriminator, out var typeForObject))
            {
                var jsonReader = jObject.CreateReader();

                if (objectType == typeForObject)
                    isInRead.Value = true;

                try
                {
                    var obj = serializer.Deserialize(jsonReader, typeForObject);
                    return obj;
                }
                finally
                {
                    isInRead.Value = false;
                }
            }

            var discriminatorName = string.IsNullOrWhiteSpace(discriminator) ? "<empty-string>" : discriminator;
            throw new JsonKnownTypesException(
                $"{discriminatorName} discriminator is not registered for {typeof(T).FullName} type");
        }

        private bool IsInWriteAndReset()
        {
            if (!isInWrite.Value) return false;
            isInWrite.Value = false;
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var objectType = value.GetType();

            if (TypesDiscriminatorValues.TryGetDiscriminator(objectType, out var discriminator))
            {
                isInWrite.Value = true;

                var writerProxy = new JsonKnownProxyWriter("type", discriminator, writer);

                try
                {
                    serializer.Serialize(writerProxy, value);
                }
                finally
                {
                    isInWrite.Value = false;
                }
            }
            else
            {
                throw new JsonKnownTypesException($"There is no discriminator for {objectType.Name} type");
            }
        }
    }
}

public static class TypesDiscriminatorValues
{
    public static bool TryGetDiscriminator(Type objectType, out string o)
    {
        o = objectType.FullName;
        return true;
    }

    public static bool TryGetType(string? discriminator, out Type o)
    {
        if (string.IsNullOrWhiteSpace(discriminator))
        {
            o = default;
            return false;
        }

        o = Type.GetType(discriminator);
        return true;
    }
}


namespace UI.JsonKnownTypes
{
    internal class JsonKnownProxyWriter : JsonWriter
    {
        private readonly string disc;
        private readonly string fieldName;
        private readonly JsonWriter writer;
        private bool discriminatorWritten;

        public JsonKnownProxyWriter(string fieldName, string disc, JsonWriter writer)
        {
            discriminatorWritten = false;
            this.fieldName = fieldName;
            this.disc = disc;
            this.writer = writer;
        }

        public override void WriteStartObject()
        {
            writer.WriteStartObject();

            if (discriminatorWritten == false)
            {
                writer.WritePropertyName(fieldName);
                writer.WriteValue(disc);
                discriminatorWritten = true;
            }
        }

        public void Dispose()
        {
            ((IDisposable) writer).Dispose();
        }

        public override Task CloseAsync(CancellationToken cancellationToken = new())
        {
            return writer.CloseAsync(cancellationToken);
        }

        public override Task FlushAsync(CancellationToken cancellationToken = new())
        {
            return writer.FlushAsync(cancellationToken);
        }

        public override Task
            WriteRawAsync(string json, CancellationToken cancellationToken = new())
        {
            return writer.WriteRawAsync(json, cancellationToken);
        }

        public override Task WriteEndAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteEndAsync(cancellationToken);
        }

        public override Task WriteEndArrayAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteEndArrayAsync(cancellationToken);
        }

        public override Task WriteEndConstructorAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteEndConstructorAsync(cancellationToken);
        }

        public override Task WriteEndObjectAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteEndObjectAsync(cancellationToken);
        }

        public override Task WriteNullAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteNullAsync(cancellationToken);
        }

        public override Task WritePropertyNameAsync(string name,
            CancellationToken cancellationToken = new())
        {
            return writer.WritePropertyNameAsync(name, cancellationToken);
        }

        public override Task WritePropertyNameAsync(string name, bool escape,
            CancellationToken cancellationToken = new())
        {
            return writer.WritePropertyNameAsync(name, escape, cancellationToken);
        }

        public override Task WriteStartArrayAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteStartArrayAsync(cancellationToken);
        }

        public override Task WriteCommentAsync(string text,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteCommentAsync(text, cancellationToken);
        }

        public override Task WriteRawValueAsync(string json,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteRawValueAsync(json, cancellationToken);
        }

        public override Task WriteStartConstructorAsync(string name,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteStartConstructorAsync(name, cancellationToken);
        }

        public override Task WriteStartObjectAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteStartObjectAsync(cancellationToken);
        }

        public override Task
            WriteValueAsync(bool value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(bool? value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(byte value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(byte? value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(byte[] value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(char value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(char? value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(DateTime value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(DateTime? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(DateTimeOffset value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(DateTimeOffset? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(decimal value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(decimal? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(double value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(double? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(float value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(float? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(Guid value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(Guid? value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(int value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(int? value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(long value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(long? value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(object value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(sbyte value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(sbyte? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(short value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(short? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(string value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(TimeSpan value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(TimeSpan? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(uint value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(uint? value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(ulong value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(ulong? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task
            WriteValueAsync(Uri value, CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(ushort value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteValueAsync(ushort? value,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteValueAsync(value, cancellationToken);
        }

        public override Task WriteUndefinedAsync(CancellationToken cancellationToken = new())
        {
            return writer.WriteUndefinedAsync(cancellationToken);
        }

        public override Task WriteWhitespaceAsync(string ws,
            CancellationToken cancellationToken = new())
        {
            return writer.WriteWhitespaceAsync(ws, cancellationToken);
        }

        public override void Flush()
        {
            writer.Flush();
        }

        public override void Close()
        {
            writer.Close();
        }

        public override void WriteEndObject()
        {
            writer.WriteEndObject();
        }

        public override void WriteStartArray()
        {
            writer.WriteStartArray();
        }

        public override void WriteEndArray()
        {
            writer.WriteEndArray();
        }

        public override void WriteStartConstructor(string name)
        {
            writer.WriteStartConstructor(name);
        }

        public override void WriteEndConstructor()
        {
            writer.WriteEndConstructor();
        }

        public override void WritePropertyName(string name)
        {
            writer.WritePropertyName(name);
        }

        public override void WritePropertyName(string name, bool escape)
        {
            writer.WritePropertyName(name, escape);
        }

        public override void WriteEnd()
        {
            writer.WriteEnd();
        }

        public override void WriteNull()
        {
            writer.WriteNull();
        }

        public override void WriteUndefined()
        {
            writer.WriteUndefined();
        }

        public override void WriteRaw(string json)
        {
            writer.WriteRaw(json);
        }

        public override void WriteRawValue(string json)
        {
            writer.WriteRawValue(json);
        }

        public override void WriteValue(string value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(int value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(uint value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(long value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(ulong value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(float value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(double value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(bool value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(short value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(ushort value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(char value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(byte value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(sbyte value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(decimal value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(DateTime value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(DateTimeOffset value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(Guid value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(TimeSpan value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(int? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(uint? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(long? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(ulong? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(float? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(double? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(bool? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(short? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(ushort? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(char? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(byte? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(sbyte? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(decimal? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(DateTime? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(DateTimeOffset? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(Guid? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(TimeSpan? value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(byte[] value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(Uri value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(object value)
        {
            writer.WriteValue(value);
        }

        public override void WriteComment(string text)
        {
            writer.WriteComment(text);
        }

        public override void WriteWhitespace(string ws)
        {
            writer.WriteWhitespace(ws);
        }
    }
}