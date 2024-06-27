using Rubytech.Data.Models;

namespace Rubytech.Providers.Interfaces
{
    public interface IDataProvider : IDisposable
    {
        public Task<IEnumerable<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<Position>> GetPositionsAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<Unit>> GetUnitsAsync(CancellationToken cancellationToken);
    }
}