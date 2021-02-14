using System;
using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2015
{
    public class _05 : IAnswer
    {
        private readonly Input _input;

        public _05(Input input)
        {
            _input = input;
        }

        public string Part1()
        {
            var nice = 0;

            foreach (var line in _input.ReadLines())
            {
                if (HasThreeVowels(line) && HasRepeatedLetter(line) && HasNoNaughtyStrings(line))
                {
                    nice++;
                }
            }

            return $"Total nice strings: {nice}.";
        }

        public string Part2()
        {
            var nice = 0;

            foreach (var line in _input.ReadLines())
            {
                if (HasRepeatedPair(line) && HasRepeatedLetterWithBuffer(line))
                {
                    nice++;
                }
            }

            return $"Total nice strings: {nice}.";
        }

        private static bool HasRepeatedPair(ReadOnlySpan<char> input)
        {
            var pairs = new Dictionary<string, List<int>>();

            for (var i = 1; i < input.Length; i++)
            {
                var pair = new string(new[] { input[i - 1], input[i] });

                if (!pairs.ContainsKey(pair))
                {
                    pairs.Add(pair, new List<int>());
                }

                var pos = i - 1;

                if (pairs[pair].Any(p => p < pos - 1))
                {
                    return true;
                }

                pairs[pair].Add(pos);
            }

            return false;
        }

        private static bool HasRepeatedLetterWithBuffer(ReadOnlySpan<char> input)
        {
            for (var i = 2; i < input.Length; i++)
            {
                if (input[i] == input[i - 2])
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasThreeVowels(ReadOnlySpan<char> input)
        {
            var vowels = 0;

            for (var i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                        if (++vowels >= 3)
                        {
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        private static bool HasRepeatedLetter(ReadOnlySpan<char> input)
        {
            char prev = ' ';

            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == prev)
                {
                    return true;
                }
                prev = input[i];
            }

            return false;
        }

        private static bool HasNoNaughtyStrings(string input) =>
            !input.Contains("ab") && !input.Contains("cd") && !input.Contains("pq") && !input.Contains("xy");
    }
}