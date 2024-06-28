namespace Rubytech.Lib.Exceptions
{
    /// <summary>
    /// Исключение неуспешной инициализации коннектора.
    /// </summary>
    /// <param name="message">Сообщение исключения.</param>
    public class InitializationFailedException(string message) : Exception(message);
}