using System.Collections.Generic;

namespace advent
{
    public static class ListExtensions
    {
        public static IList<IList<T>> GetPermutations<T>(this IList<T> list)
        {
            IList<IList<T>> result = new List<IList<T>>();

            Permute(list, list.Count, result);

            return result;
        }

        private static void Permute<T>(IList<T> list, int count, IList<IList<T>> result)
        {
            if (count == 1)
            {
                result.Add(new List<T>(list));
                return;
            }

            for (var i = 0; i < count; i++)
            {
                Permute(list, count - 1, result);
                Swap(list, count % 2 == 1 ? 0 : i, count - 1);
            }
        }

        private static void Swap<T>(IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}