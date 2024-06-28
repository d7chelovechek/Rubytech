namespace Rubytech.Archivers.Interfaces
{
    /// <summary>
    /// Предоставляет архиватор данных.
    /// </summary>
    public interface IDataArchiver : IDisposable
    {
        /// <summary>
        /// Добавить данные в файл внутри архива.
        /// </summary>
        /// <typeparam name="T">Тип, описывающий данные.</typeparam>
        /// <param name="data">Данные, необходимые для архивации.</param>
        /// <param name="fileName">Имя файла внутри архива, в котором будут данные.</param>
        /// <returns></returns>
        public Task AddDataToEntryAsync<T>(T data, string fileName);
    }
}