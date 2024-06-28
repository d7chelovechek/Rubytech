using Rubytech.Json.Converters;
using System.Text.Json.Serialization;

namespace Rubytech.Providers.Dtos
{
    public class PositionDto
    {
        [JsonConverter(typeof(LongConverter))]
        public long Id { get; set; }
        public string FullName { get; set; }
    }
}