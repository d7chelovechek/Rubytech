using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rubytech.Json.Converters
{
    /// <summary>
    /// Json конвертер для получение <see cref="DateTime"/>? из <see cref="string"/>.
    /// </summary>
    public class StringToNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            // Если токен строка и мы можем конвертировать её в дату.
            if (reader.TokenType is JsonTokenType.String &&
                DateTime.TryParse(
                    reader.GetString(), 
                    DateTimeFormatInfo.CurrentInfo, 
                    out DateTime value))
            {
                return value;
            }

            return null;
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? value, 
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}