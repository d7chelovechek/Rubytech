using Rubytech.Json.Converters;
using System.Text.Json.Serialization;

namespace Rubytech.Providers.Dtos
{
    /// <summary>
    /// Должность. 
    /// </summary>
    public class PositionDto
    {
        /// <summary>
        /// Идентификатор должности.
        /// </summary>
        [JsonConverter(typeof(LongConverter))]
        public long Id { get; set; }
        /// <summary>
        /// Наименование.
        /// </summary>
        public string FullName { get; set; }
    }
}