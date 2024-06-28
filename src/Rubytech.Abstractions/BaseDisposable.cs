namespace Rubytech.Abstractions
{
    public abstract class BaseDisposable : IDisposable
    {
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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