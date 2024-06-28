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
    /// <summary>
    /// Коннектор для системы управления учетными данными.
    /// </summary>
    public class Connector : BaseDisposable, IConnector
    {
        private readonly ILogger _logger;

        private IEnumerable<Employee>? _employees;
        private IEnumerable<Position>? _positions;
        private IEnumerable<Unit>? _units;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        /// <summary>
        /// Инициализация коннектора.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="baseUrl">Базовый адрес ресурса.</param>
        /// <param name="login">Логин для аутентификации.</param>
        /// <param name="password">Пароль для аутентификации.</param>
        /// <param name="archivePath">Путь до папки, в которую необходимо сохранить архив.</param>
        public Connector(
            ILogger logger, 
            string baseUrl, 
            string login, 
            string password, 
            string archivePath)
        {
            _logger = logger;

            // Инициализируем поставщика и архиватора данных и пытаемся получить данные.
            using var dataProvider = new RestDataProvider(baseUrl, login, password);
            using var dataArchiver = new ZipDataArchiver(archivePath, new MoscowTimeProvider());

            InitializeAsync(dataProvider, dataArchiver)
                .GetAwaiter()
                .GetResult();
        }

        // Инициализация коннектора, включающая себя получение данных и, в случае успеха, их архивацию.
        private async Task InitializeAsync(
            IDataProvider dataProvider,
            IDataArchiver dataArchiver)
        {
            if (dataProvider is null)
            {
                throw new DataProviderNotConfiguredException();
            }

            if (dataArchiver is null)
            {
                throw new DataArchiverNotConfiguredException();
            }

            await InitializeDataAsync(dataProvider);
            await SaveDataToArchiveAsync(dataArchiver);
        }

        // Сохранение полученных данных в архив.
        private async Task SaveDataToArchiveAsync(IDataArchiver dataArchiver)
        {
            // Разделяем сохранение каждой коллекции, чтобы они делали это условно параллельно.
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
            // Разделяем получение данных, чтобы они делали это условно параллельно.
            IEnumerable<Task> tasks =
            [
                InvokeCancellableTaskAsync(InitializeUnitsAsync(dataProvider)),
                InvokeCancellableTaskAsync(InitializeEmployeesAsync(dataProvider)),
                InvokeCancellableTaskAsync(InitializePositionsAsync(dataProvider))
            ];
            
            // В случае, если произошла некоторая ошибка - записываем её в лог и отдаем выше.
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

        // Попытка выполнения задачи, которую можно отменить.
        // В случае некоторой ошибки - отменяет токен, чтобы остальные задачи по получению данных остановились.
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

        // Получение должностей.
        private async Task InitializePositionsAsync(IDataProvider dataProvider)
        {
            IEnumerable<Position> positions =
                await dataProvider.GetPositionsAsync(_cancellationTokenSource.Token);

            _positions = positions.ToList();
        }

        // Получение подразделений.
        private async Task InitializeUnitsAsync(IDataProvider dataProvider)
        {
            IEnumerable<Unit> units = 
                await dataProvider.GetUnitsAsync(_cancellationTokenSource.Token);

            // Валидируем подразделения. Если валидация не пройдет - будет выброшено исключение.
            units.ValidateTree();

            _units = units.ToList();
        }

        // Получение сотрудников.
        private async Task InitializeEmployeesAsync(IDataProvider dataProvider)
        {
            IEnumerable<Employee> employees = 
                await dataProvider.GetEmployeesAsync(_cancellationTokenSource.Token);

            // Дожидаемся, когда будут получены подразделения, так как они нам нужны для валидации сотрудников.
            await TaskHelper.WhenWhile(() => _units is null, _cancellationTokenSource.Token);

            // Валидируем сотрудников. Если валидация не пройдет - будет выброшено исключение.
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