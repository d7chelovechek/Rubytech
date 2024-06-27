using System.Text.Json;

namespace Rubytech.Json.Exceptions
{
    public class InvalidConvertibleValueException() : JsonException("Неверное значение для конвертации.");
}