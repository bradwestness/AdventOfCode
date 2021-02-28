using System.Numerics;

namespace advent.Answers._2015
{
    public class _25 : IAnswer
    {
        private const ulong INITIAL_VALUE = 20151125;
        private const ulong BASE = 252533;
        private const ulong MODULUS = 33554393;

        private readonly (ulong Row, ulong Column) _input;

        public _25(Input input) => _input = ParseInput(input);

        public string Part1()
        {
            var code = GetCode(_input.Row, _input.Column);
            return $"Code at row {_input.Row} and column {_input.Column}: {code}.";
        }

        public string Part2() =>
            "There's no step two!";

        private ulong GetCode(ulong row, ulong column)
        {
            var triangle = GetTriangleNumber(row, column);
            var code = (ModularExponent(triangle) * INITIAL_VALUE) % MODULUS;
            return code;
        }

        private ulong GetTriangleNumber(ulong row, ulong column)
        {
            var side = row + column - 1;
            return (side * (side + 1)) / 2 - row;
        }

        private ulong ModularExponent(ulong exp) =>
            (ulong)BigInteger.ModPow(BASE, exp, MODULUS);

        private (ulong Row, ulong Column) ParseInput(Input input)
        {
            var str = input.ReadToEnd();
            ulong getTokenInt(string input, string token)
            {
                var substr = input.Substring(input.IndexOf(token) + token.Length);
                var (strVal, _) = substr.Split(new[] { ' ', ',', '.' }, System.StringSplitOptions.RemoveEmptyEntries);
                return ulong.Parse(strVal);
            }

            return (getTokenInt(str, "row"), getTokenInt(str, "column"));
        }
    }
}