using Microsoft.Extensions.Logging;
using Rubytech.Data.Models;
using Rubytech.Json.SerializationOptions;
using Rubytech.Lib.Exceptions;
using System.Text.Json;

namespace Rubytech.Lib.ValidatorsExtensions
{
    /// <summary>
    /// Валидатор сотрудников.
    /// </summary>
    public static class EmployeeValidatorExtension
    {
        /// <summary>
        /// Валидировать сотрудников по существующим подразделениям. 
        /// Если найдется сотрудник, у которого несуществующее подразделение, то будет выброшено исключение <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="employees">Сотрудники, которых необходимо валидировать.</param>
        /// <param name="units">Подразделения, по которым необходимо проверять</param>
        /// <returns>Сотрудники.</returns>
        /// <exception cref="ValidationException"></exception>
        public static IEnumerable<Employee> ValidateUnits(
            this IEnumerable<Employee> employees,
            IEnumerable<Unit> units)
        {
            HashSet<long> unitsIds = units.Select(u => u.Id).ToHashSet();

            foreach (Employee employee in  employees) 
            {
                // Если идентификатора подразделения пользователя нет в хэшсете идентификаторов подразделений - выбрасываем исключение.
                if (!unitsIds.Contains(employee.UnitId))
                {
                    throw new ValidationException("У сотрудника указано несуществующее подразделение.");
                }
            }

            return employees;
        }

        /// <summary>
        /// Валидировать сотрудников по дате трудоустройства.
        /// </summary>
        /// <param name="employees">Сотрудники, которых необходимо валидировать.</param>
        /// <param name="logger">Логгер, в который будут выписаны пользователи, не прошедшие валидацию.</param>
        /// <returns>Сотрудники, у которых дата трудоустройства не равна <see cref="null"/>.</returns>
        public static IEnumerable<Employee> ValidateStartDates(
            this IEnumerable<Employee> employees, 
            ILogger logger)
        {
            foreach (Employee employee in employees)
            {
                // Если дата трудоустройства равна null, то логгируем данного сотрудника и не возвращаем его.
                if (employee.StartDate is null)
                {
                    string jsonEmployee = JsonSerializer.Serialize(
                        employee, 
                        RubytechWriteSerializationOptions.Value);

                    logger?.LogInformation("У сотрудника отсутствует дата приема на работу: {JSON}", jsonEmployee);

                    continue;
                }

                yield return employee;
            }
        }
    }
}