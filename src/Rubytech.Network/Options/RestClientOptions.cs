using System.Net.Http.Headers;
using System.Text.Json;

namespace Rubytech.Network.Options
{
    /// <summary>
    /// Настойки для <see cref="Clients.RestClient"/>.
    /// </summary>
    public class RestClientOptions
    {
        /// <summary>
        /// Базовый идентификатор ресурса.
        /// </summary>
        public required Uri BaseUri { get; set; }
        /// <summary>
        /// Заголовок аутентификации.
        /// </summary>
        public required AuthenticationHeaderValue Authentication { get; set; }

        /// <summary>
        /// Настройки Json сериализатора.
        /// </summary>
        public JsonSerializerOptions? SerializationOptions { get; set; }
    }
}