using Rubytech.Data.Models;

namespace Rubytech.Lib
{
    /// <summary>
    /// Предоставляет коннектор для системы управления учетными данными.
    /// </summary>
    public interface IConnector : IDisposable
    {
        /// <summary>
        /// Получить сотрудников по идентификатору подразделения.
        /// </summary>
        /// <param name="unitId">Идентификатор подразделения.</param>
        /// <returns>Сотрудники с идентификатором подразделения равному <paramref name="unitId"/>.</returns>
        public IEnumerable<Employee> GetEmployeesByUnit(long unitId);
        /// <summary>
        /// Получить должности.
        /// </summary>
        /// <returns>Существующие должности.</returns>
        public IEnumerable<Position> GetPositions();
        /// <summary>
        /// Получить подразделение по родительскому идентификатору подразделения.
        /// </summary>
        /// <param name="parentId">Родительский идентификатор подразделения.</param>
        /// <returns>Подразделения с идентификатором родителя равному <paramref name="parentId"/>.</returns>
        public IEnumerable<Unit> GetUnitsByParentId(long parentId);
    }
}