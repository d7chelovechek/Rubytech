using AutoMapper;
using Rubytech.Abstractions;
using Rubytech.Data.Models;
using Rubytech.Json.SerializationOptions;
using Rubytech.Network.Clients;
using Rubytech.Network.Options;
using Rubytech.Providers.Constants;
using Rubytech.Providers.Dtos;
using Rubytech.Providers.Interfaces;
using Rubytech.Providers.Mappers;

namespace Rubytech.Providers
{
    /// <summary>
    /// Поставщик данных из удаленного источника.
    /// </summary>
    public class RestDataProvider : BaseDisposable, IDataProvider
    {
        private readonly RestClient _client;

        /// <summary>
        /// Инициализация поставщика данных.
        /// </summary>
        /// <param name="uriString">Базовый идентификатор ресурса.</param>
        /// <param name="userName">Имя пользователя для аутентификации.</param>
        /// <param name="password">Пароль для аутентицикации.</param>
        public RestDataProvider(
            string uriString,
            string userName,
            string password)
        {
            _client = new RestClient(new RestClientOptions()
            {
                BaseUri = new Uri(uriString),
                Authentication = new BasicAuthenticationHeaderValue(userName, password),
                SerializationOptions = RubytechReadSerializationOptions.Value
            });
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(
            CancellationToken cancellationToken)
        {
            // Получаем сотрудников из удаленного источника, маппим их и отдаем.
            var employees = await _client.GetAsync<IEnumerable<EmployeeDto>>(
                Endpoint.Employees, 
                cancellationToken);

            Mapper mapper = MapperInitializator.InitializeEmployeeMapper();

            return employees?.Select(mapper.Map<Employee>) ?? [];
        }

        public async Task<IEnumerable<Position>> GetPositionsAsync(
            CancellationToken cancellationToken)
        {
            // Получаем должности из удаленного источника, маппим их и отдаем.
            var positions = await _client.GetAsync<IEnumerable<PositionDto>>(
                Endpoint.Positions, 
                cancellationToken);

            Mapper mapper = MapperInitializator.InitializePositionMapper();

            return positions?.Select(mapper.Map<Position>) ?? [];
        }

        public async Task<IEnumerable<Unit>> GetUnitsAsync(
            CancellationToken cancellationToken)
        {
            // Получаем подразделения из удаленного источника, маппим их и отдаем.
            var units = await _client.GetAsync<IEnumerable<UnitDto>>(
                Endpoint.Units, 
                cancellationToken);

            Mapper mapper = MapperInitializator.InitializeUnitMapper();

            return units?.Select(mapper.Map<Unit>) ?? [];
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _client?.Dispose();
            }
        }
    }
}