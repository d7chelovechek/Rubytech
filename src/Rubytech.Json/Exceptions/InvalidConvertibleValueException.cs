using System.Text.Json;

namespace Rubytech.Json.Exceptions
{
    /// <summary>
    /// Исключение неверного значения для конвертации.
    /// </summary>
    public class InvalidConvertibleValueException() : JsonException("Неверное значение для конвертации.");
}