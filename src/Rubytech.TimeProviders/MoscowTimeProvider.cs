using Rubytech.TimeProviders.Interfaces;

namespace Rubytech.TimeProviders
{
    /// <summary>
    /// Предоставляет поставщика времени в Московском часовом поясе.
    /// </summary>
    public class MoscowTimeProvider : ITimeProvider
    {
        private readonly TimeProvider _timeProvider;

        private readonly TimeSpan _offset = TimeSpan.FromHours(3);

        public MoscowTimeProvider()
        {
            _timeProvider = TimeProvider.System;
        }

        public string GetCurrentDateTimeInISO8601()
        {
            // .ToString("o") нам не подходит, так как там присутствуют недопустивые для имени файла символы.
            return string.Join(
                '+',
                (_timeProvider.GetUtcNow() + _offset).ToString("yyyyMMdd'T'HHmmss"),
                _offset.ToString("hhmm"));
        }
    }
}