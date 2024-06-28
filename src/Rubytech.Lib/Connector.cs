using Microsoft.Extensions.Logging;
using Rubytech.Abstractions;
using Rubytech.Archivers;
using Rubytech.Archivers.Constants;
using Rubytech.Archivers.Interfaces;
using Rubytech.Data.Models;
using Rubytech.Lib.Exceptions;
using Rubytech.Lib.Helpers;
using Rubytech.Lib.ValidatorsExtensions;
using Rubytech.Providers;
using Rubytech.Providers.Interfaces;
using Rubytech.TimeProviders;

namespace Rubytech.Lib
{
    public class Connector : BaseDisposable, IConnector
    {
        private readonly ILogger _logger;

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

            using var dataProvider = new RestDataProvider(baseUrl, login, password);
            using var dataArchiver = new ZipDataArchiver(archivePath, new MoscowTimeProvider());

            InitializeAsync(dataProvider, dataArchiver)
                .GetAwaiter()
                .GetResult();
        }

        private async Task InitializeAsync(
            IDataProvider dataProvider,
            IDataArchiver dataArchiver)
        {
            if (dataProvider is null)
            {
                throw new DataProviderNotConfiguredException();
            }

            await InitializeDataAsync(dataProvider);
            await SaveDataToArchiveAsync(dataArchiver);
        }

        private async Task SaveDataToArchiveAsync(IDataArchiver dataArchiver)
        {
            IEnumerable<Task> tasks =
            [
                dataArchiver.AddDataToEntryAsync(_employees, FileName.Employees),
                dataArchiver.AddDataToEntryAsync(_positions, FileName.Positions),
                dataArchiver.AddDataToEntryAsync(_units, FileName.Units)
            ];

            await Task.WhenAll(tasks);
        }

        private async Task InitializeDataAsync(IDataProvider dataProvider)
        {
            IEnumerable<Task> tasks =
            [
                InvokeCancellableTaskAsync(InitializeUnitsAsync(dataProvider)),
                InvokeCancellableTaskAsync(InitializeEmployeesAsync(dataProvider)),
                InvokeCancellableTaskAsync(InitializePositionsAsync(dataProvider))
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

        private async Task InitializePositionsAsync(IDataProvider dataProvider)
        {
            IEnumerable<Position> positions =
                await dataProvider.GetPositionsAsync(_cancellationTokenSource.Token);

            _positions = positions.ToList();
        }

        private async Task InitializeUnitsAsync(IDataProvider dataProvider)
        {
            IEnumerable<Unit> units = 
                await dataProvider.GetUnitsAsync(_cancellationTokenSource.Token);

            units.ValidateTree();

            _units = units.ToList();
        }

        private async Task InitializeEmployeesAsync(IDataProvider dataProvider)
        {
            IEnumerable<Employee> employees = 
                await dataProvider.GetEmployeesAsync(_cancellationTokenSource.Token);

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
                _cancellationTokenSource?.Dispose();
            }
        }
    }
}