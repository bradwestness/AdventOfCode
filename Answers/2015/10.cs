using System;
using System.Text;

namespace advent.Answers._2015
{
    public class _10 : IAnswer
    {
        private readonly string _input;

        public _10(Input input)
        {
            _input = input.ReadToEnd()?.Trim() ?? string.Empty;
        }

        public string Part1()
        {
            string result = _input;

            for (var i = 0; i < 40; i++)
            {
                result = LookAndSay(result);
            }

            return $"Result length: {result.Length}";
        }

        public string Part2()
        {
            string result = _input;

            for (var i = 0; i < 50; i++)
            {
                result = LookAndSay(result);
            }

            return $"Result length: {result.Length}";
        }

        private StringBuilder _sb = new StringBuilder();

        private string LookAndSay(ReadOnlySpan<char> input)
        {
            _sb.Clear();

            for (var i = 0; i < input.Length; i++)
            {
                var current = input[i];
                var count = 1;
                char? next = (i < input.Length - 1)
                    ? input[i + 1]
                    : null;

                while (current == next && i < input.Length)
                {
                    i++;
                    count++;
                    next = (i < input.Length - 1)
                        ? input[i + 1]
                        : null;
                }

                _sb.Append($"{count}{current}");
            }

            return _sb.ToString();
        }
    }
}