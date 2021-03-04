using System;
using advent.Input;

namespace advent.Answers._2015
{
    public class _01 : IPuzzleAnswer
    {
        private readonly string _input;

        public _01(IPuzzleInput input) => _input = input.ReadToEnd();

        public string Part1()
        {
            var floor = 0;
            ReadOnlySpan<char> chars = _input;

            for (var pos = 0; pos < chars.Length; pos++)
            {
                switch (chars[pos])
                {
                    case '(':
                        floor += 1;
                        break;

                    case ')':
                        floor -= 1;
                        break;

                    default:
                        throw new Exception($"Unexpected char at position {pos}: {chars[pos]}!");
                }
            }

            return $"Arrived at floor {floor}.";
        }

        public string Part2()
        {
            var floor = 0;
            ReadOnlySpan<char> chars = _input;

            for (var pos = 0; pos < chars.Length; pos++)
            {
                switch (chars[pos])
                {
                    case '(':
                        floor += 1;
                        break;

                    case ')':
                        floor -= 1;
                        if (floor < 0)
                        {
                            return $"Going to the basement for the first time at position {pos + 1}.";
                        }
                        break;

                    default:
                        throw new Exception($"Unexpected char at position {pos}: {chars[pos]}!");
                }
            }

            return $"Never went to the basement.";
        }
    }
}