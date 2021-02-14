using System;
using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2015
{
    public class _11 : IAnswer
    {
        private readonly string _input;

        public _11(Input input)
        {
            _input = input.ReadToEnd();
        }

        public string Part1()
        {
            var first = FindNextValidPassword(_input);

            return $"First valid password: {first}";
        }

        public string Part2()
        {
            var first = FindNextValidPassword(_input);
            var second = FindNextValidPassword(first);

            return $"Second valid password: {second}";
        }

        private string FindNextValidPassword(string password)
        {
            var newPassword = Increment(password);

            while (!IsValid(newPassword))
            {
                newPassword = Increment(newPassword);
            }

            return newPassword;
        }

        private string Increment(ReadOnlySpan<char> input)
        {
            var output = new char[input.Length];
            char incrementChar(char c) => (int)c switch
            {
                // when the char is ascii 'a' through 'y', increment by 1
                int n when (n >= 97 && n <= 121) => (char)(n + 1),
                // when the char is ascii 'z', set it to ascii a
                122 => (char)97,
                // otherwise just return the original character
                _ => c
            };

            for (var i = input.Length - 1; i >= 0; i--)
            {
                output[i] = incrementChar(input[i]);

                if ((int)output[i] >= (int)input[i])
                {
                    // we incremented a character that didn't wrap around,
                    // so we're done

                    for (var j = i - 1; j >= 0; j--)
                    {
                        output[j] = input[j];
                    }

                    return new string(output);
                }
            }

            // the first character wrapped around, so
            // we need to prefix an 'a'
            return new string(output.Prepend('a').ToArray());
        }

        private bool IsValid(string pwd) =>
            HasIncreasingStraight(pwd, 3) &&
            !HasForbiddenCharacter(pwd, 'i', 'o', 'l') &&
            HasNonOverlappingPairs(pwd, 2);

        private bool HasIncreasingStraight(ReadOnlySpan<char> input, int length)
        {
            for (var i = 0; i < input.Length; i++)
            {
                var code = (int)input[i];
                var straight = 1;
                var nextCode = input.Length > i + 1
                    ? (int)input[i + 1]
                    : -1;

                var letter = input[i];
                var nextLetter = nextCode > -1 ? (char)nextCode : ' ';

                while (nextCode == code + 1 && input.Length > i + 1)
                {
                    straight++;

                    if (straight >= 3)
                    {
                        return true;
                    }

                    i++;
                    code = (int)input[i];
                    nextCode = input.Length > i + 1
                        ? (int)input[i + 1]
                        : -1;
                }
            }

            return false;
        }

        private bool HasForbiddenCharacter(ReadOnlySpan<char> input, params char[] forbidden)
        {
            ReadOnlySpan<char> outer = input;
            ReadOnlySpan<char> inner = forbidden;

            // use the smaller of the two spans as the outer array
            if (forbidden.Length > input.Length)
            {
                outer = forbidden;
                inner = input;
            }

            foreach (var o in outer)
            {
                foreach (var i in inner)
                {
                    if (o == i)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasNonOverlappingPairs(ReadOnlySpan<char> input, int minimum)
        {
            // this list will accumulate the position of pairs within
            // the string, which will be used to ensure we don't count
            // overlapping pairs (e.g. 'aaa' is one pair, not two)
            var pairs = new List<int>();

            for (var pos = 0; pos < input.Length; pos++)
            {
                char? next = input.Length > pos + 1
                    ? input[pos + 1]
                    : null;

                if (next.HasValue && next == input[pos] && !pairs.Any(p => p == pos - 1))
                {
                    pairs.Add(pos);
                }

                if (pairs.Count >= minimum)
                {
                    return true;
                }
            }

            return false;
        }
    }
}