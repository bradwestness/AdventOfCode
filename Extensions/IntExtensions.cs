namespace advent
{
    public static class IntExtensions
    {
        public static bool IsEven(this int number) => (number & 1) == 0;

        public static bool IsOdd(this int number) => (number & 1) != 0;
    }
}