using Rubytech.Data.Models;
using Rubytech.Lib.Exceptions;

namespace Rubytech.Lib.ValidatorsExtensions
{
    public static class UnitsValidatorExtension
    {
        public static IEnumerable<Unit> ValidateTree(this IEnumerable<Unit> units)
        {
            Unit? rootUnit = null;
            
            try
            {
                rootUnit = units.SingleOrDefault(e => e.ParentId is null);
            }
            catch
            {
                throw new ValidationException("Может быть только одно главное подразделение.");
            }

            if (rootUnit is null)
            {
                throw new ValidationException("Не найдено главное подразделение.");
            }

            var visitedUnits = new HashSet<long>();
            var unitsStack = new Stack<long>();

            unitsStack.Push(rootUnit.Id);

            while (unitsStack.Count > 0)
            {
                long unitId = unitsStack.Pop();

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

            if (visitedUnits.Count != units.Count())
            {
                throw new ValidationException("Не все подразделения находятся в дереве подразделений.");
            }

            return units;
        }
    }
}