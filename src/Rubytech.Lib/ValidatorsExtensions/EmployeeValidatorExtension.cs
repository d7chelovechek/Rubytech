using Microsoft.Extensions.Logging;
using Rubytech.Data.Models;
using Rubytech.Lib.Exceptions;
using System.Text.Json;

namespace Rubytech.Lib.ValidatorsExtensions
{
    public static class EmployeeValidatorExtension
    {
        public static IEnumerable<Employee> ValidateUnits(
            this IEnumerable<Employee> employees,
            IEnumerable<Unit> units)
        {
            HashSet<long> unitsIds = units.Select(u => u.Id).ToHashSet();

            foreach (Employee employee in  employees) 
            {
                if (!unitsIds.Contains(employee.UnitId))
                {
                    throw new ValidationException("У сотрудника указано несуществующее подразделение.");
                }
            }

            return employees;
        }

        public static IEnumerable<Employee> ValidateStartDates(
            this IEnumerable<Employee> employees, 
            ILogger logger)
        {
            foreach (Employee employee in employees)
            {
                if (employee.StartDate is null)
                {
                    string jsonEmployee = JsonSerializer.Serialize(employee);

                    logger?.LogInformation("У сотрудника отсутствует дата приема на работу: {JSON}", jsonEmployee);

                    continue;
                }

                yield return employee;
            }
        }
    }
}