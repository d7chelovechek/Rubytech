namespace Rubytech.Archivers.Interfaces
{
    public interface IDataArchiver : IDisposable
    {
        public Task AddDataToEntryAsync<T>(T data, string fileName);
    }
}