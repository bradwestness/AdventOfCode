using System.Collections.Generic;
using System.Linq;

namespace advent
{
    public static class ListExtensions
    {
        public static IList<IList<T>> GetPermutations<T>(this IList<T> list)
        {
            List<IList<T>> permutations = new();
            var stack = new int[list.Count];
            stack.Initialize();

            var i = 0;
            while (i < list.Count)
            {
                if (stack[i] < i)
                {
                    Swap(ref list, i.IsEven() ? 0 : stack[i], i);
                    permutations.Add(list.ToList());
                    stack[i]++;
                    i = 0;
                }
                else
                {
                    stack[i] = 0;
                    i++;
                }
            }

            return permutations;
        }

        private static void Swap<T>(ref IList<T> list, int indexA, int indexB)
        {
            var temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        public static IEnumerable<IList<T>> GetCombinations<T>(this IList<T> list, int minItems = 1, int? maxItems = null)
        {
            if (!maxItems.HasValue)
            {
                maxItems = list.Count;
            }

            var maxCombinations = 1 << list.Count;

            for (var i = 1; i <= maxCombinations; i++)
            {
                var combination = list.GetCombination(i, minItems, maxItems);

                if (combination is object)
                {
                    yield return combination;
                }
            }
        }

        public static IList<T> GetCombination<T>(this IList<T> list, int combinationNumber, int minItems = 1, int? maxItems = null)
        {
            if (!maxItems.HasValue)
            {
                maxItems = list.Count;
            }

            List<T> combination = new();

            for (var i = 0; i < list.Count; i++)
            {
                if ((combinationNumber >> i).IsOdd())
                {
                    if (combination.Count == maxItems)
                    {
                        // this combination would have
                        // more than the specified maximum
                        // number of items, so return null
                        return null;
                    }

                    combination.Add(list[i]);
                }
            }

            if (combination.Count < minItems)
            {
                // this combination has fewer than the
                // specified minimum number of items,
                // so return null
                return null;
            }

            return combination;
        }
    }
}