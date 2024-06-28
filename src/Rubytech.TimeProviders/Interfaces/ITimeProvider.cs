namespace Rubytech.TimeProviders.Interfaces
{
    /// <summary>
    /// Предоставляет поставщика времени.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Получить текущие дату и время в формате ISO 8601.
        /// </summary>
        /// <returns>Строка даты и времени в ISO 8601 формате.</returns>
        public string GetCurrentDateTimeInISO8601();
    }
}