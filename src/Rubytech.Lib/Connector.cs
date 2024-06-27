using Microsoft.Extensions.Logging;
using Rubytech.Abstractions.BaseObjects;
using Rubytech.Data.Models;
using Rubytech.Lib.Exceptions;
using Rubytech.Lib.Helpers;
using Rubytech.Lib.ValidatorsExtensions;
using Rubytech.Providers;
using Rubytech.Providers.Interfaces;

namespace Rubytech.Lib
{
    public class Connector : BaseDisposable, IConnector
    {
        private readonly ILogger _logger;

        private readonly IDataProvider _dataProvider;

        private IEnumerable<Employee>? _employees;
        private IEnumerable<Position>? _positions;
        private IEnumerable<Unit>? _units;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public Connector(
            ILogger logger, 
            string baseUrl, 
            string login, 
            string password, 
            string archivePath)
        {
            _logger = logger;
            _dataProvider = new RestDataProvider(baseUrl, login, password);

            InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            if (_dataProvider is null)
            {
                throw new DataProviderNotConfiguredException();
            }

            await InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            IEnumerable<Task> tasks =
            [
                InvokeCancellableTaskAsync(InitializeUnitsAsync()),
                InvokeCancellableTaskAsync(InitializeEmployeesAsync()),
                InvokeCancellableTaskAsync(InitializePositionsAsync())
            ];

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Произошла ошибка при инициализации коннектора: {Message}", ex.Message);

                throw new InitializationFailedException(ex.Message);
            }
        }

        private async Task InvokeCancellableTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                _cancellationTokenSource.Cancel();

                throw;
            }
        }

        private async Task InitializePositionsAsync()
        {
            IEnumerable<Position> positions =
                await _dataProvider.GetPositionsAsync(_cancellationTokenSource.Token);

            _positions = positions.ToList();
        }

        private async Task InitializeUnitsAsync()
        {
            IEnumerable<Unit> units = 
                await _dataProvider.GetUnitsAsync(_cancellationTokenSource.Token);

            units.ValidateTree();

            _units = units.ToList();
        }

        private async Task InitializeEmployeesAsync()
        {
            IEnumerable<Employee> employees = 
                await _dataProvider.GetEmployeesAsync(_cancellationTokenSource.Token);

            await TaskHelper.WhenUntil(() => _units is null, _cancellationTokenSource.Token);

            employees = employees
                .ValidateUnits(_units!)
                .ValidateStartDates(_logger);

            _employees = employees.ToList();
        }

        public IEnumerable<Employee> GetEmployeesByUnit(long unitId)
        {
            return _employees!.Where(e => e.UnitId == unitId);
        }

        public IEnumerable<Position> GetPositions()
        {
            return _positions!;
        }

        public IEnumerable<Unit> GetUnitsByParentId(long parentId)
        {
            return _units!.Where(u =>  u.ParentId == parentId);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _dataProvider?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
        }
    }
}