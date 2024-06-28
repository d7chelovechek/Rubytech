using Rubytech.TimeProviders.Interfaces;

namespace Rubytech.TimeProviders
{
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
            return string.Join(
                '+',
                (_timeProvider.GetUtcNow() + _offset).ToString("yyyyMMdd'T'HHmmss"),
                _offset.ToString("hhmm"));
        }
    }
}