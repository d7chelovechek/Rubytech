namespace Rubytech.Lib.Helpers
{
    public static class TaskHelper
    {
        public static async Task WhenUntil(
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