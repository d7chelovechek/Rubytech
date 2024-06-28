using Rubytech.Abstractions;
using Rubytech.Network.Options;
using System.Text.Json;

namespace Rubytech.Network.Clients
{
    /// <summary>
    /// Обертку над <see cref="HttpClient"/>
    /// </summary>
    public class RestClient : BaseDisposable
    {
        private readonly HttpClient _client;

        private readonly JsonSerializerOptions _serializationOptions;

        /// <summary>
        /// Инициализация клиента.
        /// </summary>
        /// <param name="options">Настройки клиента.</param>
        public RestClient(RestClientOptions options)
        {
            _serializationOptions = options.SerializationOptions ?? JsonSerializerOptions.Default;

            _client = new HttpClient()
            {
                BaseAddress = options.BaseUri
            };

            _client.DefaultRequestHeaders.Authorization = options.Authentication;
        }

        /// <summary>
        /// Отправить GET запрос.
        /// </summary>
        /// <typeparam name="TData">Тип возвращаемых данных.</typeparam>
        /// <param name="endpoint">Конечная точка для отправки запроса.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Десериализованные данные.</returns>
        public async Task<TData?> GetAsync<TData>(
            string endpoint, 
            CancellationToken cancellationToken)
        {
            // Отправка запроса.
            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            using HttpResponseMessage response = await _client.SendAsync(request, cancellationToken);

            // Выкидываем исключение, если у нас не успешный код.
            response.EnsureSuccessStatusCode();

            // Читаем и десериализуем данные.
            string json = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<TData>(json, _serializationOptions);
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