using System;

namespace advent.Answers._2015
{
    public class _08 : IAnswer
    {
        private readonly string _input;

        public _08(string input)
        {
            _input = input;
        }

        public string Part1()
        {
            var totalChars = 0;
            var totalUnencodedChars = 0;

            foreach (var line in _input.ToLines())
            {
                var (charCount, unencodedCharCount) = CountUnencodedCharacters(line);
                totalChars += charCount;
                totalUnencodedChars += unencodedCharCount;
            }

            var difference = totalChars - totalUnencodedChars;

            return $"Total chars: {totalChars}, total unencoded chars: {totalUnencodedChars}, difference: {difference}.";
        }

        public string Part2()
        {
            var totalChars = 0;
            var totalEncodedChars = 0;

            foreach (var line in _input.ToLines())
            {
                var (charCount, encodedCharCount) = CountEncodedCharacters(line);
                totalChars += charCount;
                totalEncodedChars += encodedCharCount;
            }

            var difference = totalEncodedChars - totalChars;

            return $"Total chars: {totalChars}, total encoded chars: {totalEncodedChars}, difference: {difference}.";
        }

        private static (int CharCount, int UnencodedCharCount) CountUnencodedCharacters(ReadOnlySpan<char> line)
        {
            var chars = line.Trim();
            var unencodedCharCount = 0;

            for (var i = 1; i < chars.Length - 1; i++)
            {
                unencodedCharCount++;

                if (chars[i] == '\\')
                {
                    switch (chars[i + 1])
                    {
                        case '\\':
                        case '\"':
                            i++;
                            break;

                        case 'x':
                            i += 3;
                            break;
                    }
                }
            }

            return (chars.Length, unencodedCharCount);
        }

        private (int CharCount, int EncodedCharCount) CountEncodedCharacters(ReadOnlySpan<char> line)
        {
            var chars = line.Trim();
            var encodedCharCount = chars.Length;

            // increase the count by one for every escape character in the line
            foreach (char c in chars)
            {
                switch (c)
                {
                    case '\\':
                    case '\"':
                        encodedCharCount++;
                        break;
                }
            }

            // increase the count by two to simulate enclosing quotes
            encodedCharCount += 2;

            return (chars.Length, encodedCharCount);
        }
    }
}