namespace Rubytech.Lib.Exceptions
{
    /// <summary>
    /// Исключение валидации данных.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    public class ValidationException(string message): Exception(message);
}