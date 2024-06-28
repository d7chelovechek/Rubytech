using Rubytech.Data.Enums;
using Rubytech.Json.Converters;
using System.Text.Json.Serialization;

namespace Rubytech.Providers.Dtos
{
    public class EmployeeDto
    {
        [JsonConverter(typeof(LongConverter))]
        public long Id { get; set; }

        [JsonConverter(typeof(LongToStringConverter))]
        public string PersonnelNumber { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }

        [JsonIgnore]
        public string FullName =>
            string.Join(' ', LastName, FirstName, Patronymic);

        [JsonIgnore]
        public WorkStatus Status =>
            DissmissedAt is null ? WorkStatus.Work : WorkStatus.Dismissed;

        [JsonConverter(typeof(StringToLongConverter))]
        public long PositionId { get; set; }
        [JsonConverter(typeof(StringToLongConverter))]
        public long UnitId { get; set; }

        [JsonConverter(typeof(IntToNullableBooleanConverter))]
        public bool? IsMain { get; set; }

        [JsonConverter(typeof(StringToNullableDateTimeConverter))]
        public DateTime? DateFrom { get; set; }
        [JsonConverter(typeof(StringToNullableDateTimeConverter))]
        public DateTime? DissmissedAt { get; set; }
    }
}