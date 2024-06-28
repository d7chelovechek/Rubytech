using Rubytech.Data.Models;
using Rubytech.Lib.Exceptions;

namespace Rubytech.Lib.ValidatorsExtensions
{
    /// <summary>
    /// Валидатор подразделений.
    /// </summary>
    public static class UnitsValidatorExtension
    {
        /// <summary>
        /// Валидировать сотрудников по правильно построенному дереву. 
        /// Если дерево поздразделений построено неправильно, то будет выброшено исключение <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="units">Подразделения, которые необходимо валидировать.</param>
        /// <returns>Подразделения.</returns>
        /// <exception cref="ValidationException"></exception>
        public static IEnumerable<Unit> ValidateTree(this IEnumerable<Unit> units)
        {
            Unit? rootUnit = null;
            
            // Проверяем на единственное главное поздразделение. Если оно не одно - выбрасываем исключение.
            try
            {
                rootUnit = units.SingleOrDefault(e => e.ParentId is null);
            }
            catch
            {
                throw new ValidationException("Может быть только одно главное подразделение.");
            }

            // Если ни одного главного поздразделения не найдет, то выбрасываем исключение.
            if (rootUnit is null)
            {
                throw new ValidationException("Не найдено главное подразделение.");
            }

            var visitedUnits = new HashSet<long>();
            var unitsStack = new Stack<long>();

            unitsStack.Push(rootUnit.Id);

            // Проходимся по всем подразделениям.
            while (unitsStack.Count > 0)
            {
                long unitId = unitsStack.Pop();

                // Если мы повторно посещаем подразделение - значит дубликат, необходимо выбросить исключение.
                if (!visitedUnits.Add(unitId))
                {
                    throw new ValidationException("Повторение подразделения в дереве подразделений.");
                }

                foreach (var childId in units.Where(e => e.ParentId == unitId).Select(e => e.Id))
                {
                    if (!visitedUnits.Contains(childId))
                    {
                        unitsStack.Push(childId);
                    }
                }
            }

            // Если количество посещенных подразделений не равна изначальному их количеству - выбрасываем исключение.
            if (visitedUnits.Count != units.Count())
            {
                throw new ValidationException("Не все подразделения находятся в дереве подразделений.");
            }

            return units;
        }
    }
}