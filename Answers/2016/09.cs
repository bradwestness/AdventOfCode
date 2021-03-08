using System;
using advent.Input;

namespace advent.Answers._2016
{
    public class _09 : IPuzzleAnswer
    {
        private string _input;

        public _09(IPuzzleInput input) => _input = input.ReadToEnd();

        public string Part1()
        {
            var length = GetDecompressedLength(_input);
            return $"Decompressed length: {length}.";
        }

        public string Part2()
        {
            var length = GetDecompressedLength(_input, allowRecursion: true);
            return $"Decompressed length: {length}.";
        }

        private long GetDecompressedLength(ReadOnlySpan<char> input, bool allowRecursion = false)
        {
            long length = input.Length;
            var i = 0;

            while (i < input.Length)
            {
                if (input[i] != '(')
                {
                    i++;
                    continue;
                }

                var marker = input.Slice(i, input.Slice(i).IndexOf(')') + 1);
                var xIndex = marker.IndexOf('x');
                var numChars = int.Parse(marker.Slice(1, xIndex - 1));
                var numRepeats = int.Parse(marker.Slice(xIndex + 1, marker.IndexOf(')') - xIndex - 1));
                var dataLength = allowRecursion
                    ? GetDecompressedLength(input.Slice(i + marker.Length, numChars), true)
                    : numChars;

                length += dataLength * numRepeats - numChars - marker.Length;
                i += marker.Length + numChars;
            }

            return length;
        }
    }
}