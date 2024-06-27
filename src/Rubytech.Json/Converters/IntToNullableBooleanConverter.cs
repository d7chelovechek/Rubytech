using Rubytech.Json.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rubytech.Json.Converters
{
    public class IntToNullableBooleanConverter : JsonConverter<bool?>
    {
        public override bool? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Number &&
                reader.TryGetInt32(out int value))
            {
                return value switch
                {
                    0 => false,
                    1 => true,
                    _ => throw new InvalidConvertibleValueException()
                };
            }

            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, 
            bool? value, 
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}