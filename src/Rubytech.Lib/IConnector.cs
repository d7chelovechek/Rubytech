using Rubytech.Data.Models;

namespace Rubytech.Lib
{
    public interface IConnector : IDisposable
    {
        public IEnumerable<Employee> GetEmployeesByUnit(long unitId);
        public IEnumerable<Position> GetPositions();
        public IEnumerable<Unit> GetUnitsByParentId(long parentId);
    }
}