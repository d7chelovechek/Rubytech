﻿using Rubytech.Data.Enums;

namespace Rubytech.Data.Models
{
    public class Employee
    {
        public long Id { get; set; }

        // Табельный номер
        public string PersonnelNumber { get; set; }
        public string FullName { get; set; }
        public WorkStatus Status { get; set; }

        // Должность
        public long PositionId { get; set; }

        // Подразделение
        public long UnitId { get; set; }

        // Основная или по совместительству
        public bool? IsMainJob { get; set; }

        // Дата приема на работу
        public DateTime? StartDate { get; set; }
    }
}