namespace Rubytech.Lib.Helpers
{
    /// <summary>
    /// Помощник для работы с задачами.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// Ожидать пока выполняется условие.
        /// </summary>
        /// <param name="predicate">Условие ожидания. Пока <see cref="true"/> - будем ждать.</param>
        /// <param name="cancellationToken">Токен отмены ожидания.</param>
        /// <param name="delay">Задержка проверки условия в миллисекундах.</param>
        /// <returns></returns>
        public static async Task WhenWhile(
            Func<bool> predicate,  
            CancellationToken cancellationToken,
            int delay = 50)
        {
            while (predicate())
            {
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}