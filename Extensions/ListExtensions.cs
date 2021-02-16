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
                    if (i.IsEven())
                    {
                        Swap(ref list, 0, i);
                    }
                    else
                    {
                        Swap(ref list, stack[i], i);
                    }

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

        private static void Swap<T>(ref IList<T> list, int a, int b)
        {
            var temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        public static IList<IList<T>> GetCombinations<T>(this IList<T> list, int minItems = 1, int? maxItems = null)
        {
            if (!maxItems.HasValue)
            {
                maxItems = list.Count;
            }

            List<IList<T>> combinations = new();
            var max = 1 << list.Count;

            for (var i = 1; i <= max; i++)
            {
                List<T> combination = new();

                for (var j = 0; j < list.Count; j++)
                {
                    if ((i >> j).IsOdd())
                    {
                        combination.Add(list[j]);
                    }
                }

                if (combination.Count >= minItems &&
                    combination.Count <= maxItems.Value)
                {
                    combinations.Add(combination);
                }
            }

            return combinations;
        }
    }
}