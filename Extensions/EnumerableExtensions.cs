using System.Collections.Generic;
using System.Linq;

namespace advent
{
    public static class EnumerableExtensions
    {
        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out IEnumerable<T> rest)
        {
            first = enumerable.FirstOrDefault();
            rest = enumerable.Skip(1);
        }

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out IEnumerable<T> rest) => (first, (second, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out IEnumerable<T> rest) => (first, second, (third, rest)) = enumerable;
    }
}