using Rubytech.Json.Converters;
using System.Text.Json.Serialization;

namespace Rubytech.Providers.Dtos
{
    public class UnitDto
    {
        public long Id { get; set; }
        [JsonConverter(typeof(StringToNullableLongConverter))]
        public long? ParentId { get; set; }
        public string Name { get; set; }
    }
}