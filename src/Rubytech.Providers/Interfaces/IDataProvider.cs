using Rubytech.Data.Models;

namespace Rubytech.Providers.Interfaces
{
    /// <summary>
    /// Предоставляет поставщика данных.
    /// </summary>
    public interface IDataProvider : IDisposable
    {
        /// <summary>
        /// Получить коллекцию сотрудников.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены получения данных.</param>
        /// <returns>Коллекция сотрудников.</returns>
        public Task<IEnumerable<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Получить коллекцию должностей.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены получения данных.</param>
        /// <returns>Коллекция должностей.</returns>
        public Task<IEnumerable<Position>> GetPositionsAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Получить коллекцию подразделеений.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены получения данных.</param>
        /// <returns>Коллекция подразделений.</returns>
        public Task<IEnumerable<Unit>> GetUnitsAsync(CancellationToken cancellationToken);
    }
}