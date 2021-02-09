using System.Collections.Generic;
using System.Linq;

namespace advent
{
    public static class ListExtensions
    {
        public static IList<IList<T>> GetPermutations<T>(this IList<T> list)
        {
            if (list.Count == 1)
            {
                return new List<IList<T>>(new[] { list });
            }

            var permutations = new List<IList<T>>();

            foreach (var item in list)
            {
                var others = list.Except(new[] { item }).ToList();
                var perms = GetPermutations(others).Select(p => p.Prepend(item).ToList());
                permutations.AddRange(perms);
            }

            return permutations;
        }
    }
}