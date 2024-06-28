namespace Rubytech.Abstractions
{
    /// <summary>
    /// Базовый класс Disposable объекта
    /// </summary>
    public abstract class BaseDisposable : IDisposable
    {
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Освободить неуправляемые ресурсы.
        /// </summary>
        /// <param name="disposing">Исходит ли вызов метода из <see cref="Dispose()"/> или из финализатора.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }

        ~BaseDisposable()
        {
            Dispose(false);
        }
    }
}