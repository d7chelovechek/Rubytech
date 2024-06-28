using Rubytech.Data.Enums;
using Rubytech.Json.Converters;
using System.Text.Json.Serialization;

namespace Rubytech.Providers.Dtos
{
    /// <summary>
    /// Сотрудник.
    /// </summary>
    public class EmployeeDto
    {
        /// <summary>
        /// Идентификатор сотрудника.
        /// </summary>
        [JsonConverter(typeof(LongConverter))]
        public long Id { get; set; }

        /// <summary>
        /// Персональный номер.
        /// </summary>
        [JsonConverter(typeof(LongToStringConverter))]
        public string PersonnelNumber { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Полное имя.
        /// </summary>
        [JsonIgnore]
        public string FullName =>
            string.Join(' ', LastName, FirstName, Patronymic);

        /// <summary>
        /// Текущий статус работы.
        /// </summary>
        [JsonIgnore]
        public WorkStatus Status =>
            DissmissedAt is null ? WorkStatus.Work : WorkStatus.Dismissed;

        /// <summary>
        /// Идентификатор должности.
        /// </summary>
        [JsonConverter(typeof(StringToLongConverter))]
        public long PositionId { get; set; }
        /// <summary>
        /// Идентификатор подразделения.
        /// </summary>
        [JsonConverter(typeof(StringToLongConverter))]
        public long UnitId { get; set; }

        /// <summary>
        /// Оснавная ли это работа.
        /// </summary>
        [JsonConverter(typeof(IntToNullableBooleanConverter))]
        public bool? IsMain { get; set; }

        /// <summary>
        /// Дата трудоустройства.
        /// </summary>
        [JsonConverter(typeof(StringToNullableDateTimeConverter))]
        public DateTime? DateFrom { get; set; }
        /// <summary>
        /// Дата увольнения.
        /// </summary>
        [JsonConverter(typeof(StringToNullableDateTimeConverter))]
        public DateTime? DissmissedAt { get; set; }
    }
}