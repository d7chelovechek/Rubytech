using Rubytech.Abstractions.BaseObjects;
using Rubytech.Network.Options;
using System.Text.Json;

namespace Rubytech.Network.Clients
{
    public class RestClient : BaseDisposable
    {
        private readonly HttpClient _client;

        private readonly JsonSerializerOptions _serializationOptions;

        public RestClient(RestClientOptions options)
        {
            _serializationOptions = options.SerializationOptions ?? JsonSerializerOptions.Default;

            _client = new HttpClient()
            {
                BaseAddress = options.BaseUri
            };

            _client.DefaultRequestHeaders.Authorization = options.Authentication;
        }

        public async Task<TData?> GetAsync<TData>(
            string endpoint, 
            CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            using HttpResponseMessage response = await _client.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

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