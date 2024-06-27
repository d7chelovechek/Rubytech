using System.Net.Http.Headers;
using System.Text.Json;

namespace Rubytech.Network.Options
{
    public class RestClientOptions
    {
        public required Uri BaseUri { get; set; }
        public required AuthenticationHeaderValue Authentication { get; set; }

        public JsonSerializerOptions? SerializationOptions { get; set; }
    }
}