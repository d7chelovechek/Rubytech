using Rubytech.Json.Converters;
using System.Text.Json.Serialization;

namespace Rubytech.Providers.Dtos
{
    /// <summary>
    /// Подразделение.
    /// </summary>
    public class UnitDto
    {
        /// <summary>
        /// Идентификатор подразделения.
        /// </summary>
        [JsonConverter(typeof(LongConverter))]
        public long Id { get; set; }
        /// <summary>
        /// Идентификатор родительского подразделения.
        /// </summary>
        [JsonConverter(typeof(StringToNullableLongConverter))]
        public long? ParentId { get; set; }
        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }
    }
}