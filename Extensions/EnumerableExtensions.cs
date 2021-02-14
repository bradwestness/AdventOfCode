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

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out IEnumerable<T> rest) => (first, second, third, (fourth, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out IEnumerable<T> rest) => (first, second, third, fourth, (fifth, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, (sixth, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, (seventh, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out T eighth,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, seventh, (eighth, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out T eighth,
             out T ninth,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, seventh, eighth, (ninth, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out T eighth,
             out T ninth,
             out T tenth,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, (tenth, rest)) = enumerable;

        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out T eighth,
             out T ninth,
             out T tenth,
             out T eleventh,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, (eleventh, rest)) = enumerable;
             
        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out T eighth,
             out T ninth,
             out T tenth,
             out T eleventh,
             out T twelfth,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, (twelfth, rest)) = enumerable;
             
        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out T eighth,
             out T ninth,
             out T tenth,
             out T eleventh,
             out T twelfth,
             out T thirteenth,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, (thirteenth, rest)) = enumerable;
             
        public static void Deconstruct<T>(
            this IEnumerable<T> enumerable,
             out T first,
             out T second,
             out T third,
             out T fourth,
             out T fifth,
             out T sixth,
             out T seventh,
             out T eighth,
             out T ninth,
             out T tenth,
             out T eleventh,
             out T twelfth,
             out T thirteenth,
             out T fourteenth,
             out IEnumerable<T> rest) => (first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, (fourteenth, rest)) = enumerable;
             
    }
}