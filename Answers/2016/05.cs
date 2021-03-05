using System.Linq;
using advent.Input;

namespace advent.Answers._2016
{
    public class _05 : IPuzzleAnswer
    {
        private readonly string _doorId;

        public _05(IPuzzleInput input) => _doorId = input.ReadToEnd();

        public string Part1()
        {
            var password = GetPassword1(_doorId, 8, 5);
            return $"The password is: {password}.";
        }

        public string Part2()
        {
            var password = GetPassword2(_doorId, 8, 5);
            return $"The password is: {password}.";
        }

        private string GetPassword1(string doorId, int length, int precedingZeroes)
        {
            var hashPrefix = GetHashPrefix('0', precedingZeroes);
            var chars = new char[length];
            var index = 0;
            char c;

            for (var i = 0; i < length; i++)
            {
                (index, c) = GetNextCharacter1(doorId, index, hashPrefix);
                chars[i] = c;
            }

            return new string(chars);
        }

        private (int NextIndex, char NextChar) GetNextCharacter1(string doorId, int index, string hashPrefix)
        {
            string hash;

            do
            {
                hash = doorId.GetMD5Hash(prefix: null, suffix: index.ToString());
                index++;
            } while (!hash.StartsWith(hashPrefix));

            return (index, hash[hashPrefix.Length]);
        }

        private string GetPassword2(string doorId, int length, int precedingZeroes)
        {
            var hashPrefix = GetHashPrefix('0', precedingZeroes);
            var chars = new char[length];
            var index = 0;
            char character;
            int charPos;

            while (chars.Any(c => c == default))
            {
                (index, character, charPos) = GetNextCharacter2(doorId, index, hashPrefix, length);

                if (chars[charPos] == default)
                {
                    chars[charPos] = character;
                }
            }

            return new string(chars);
        }

        private (int NextIndex, char NextChar, int CharPosition) GetNextCharacter2(string doorId, int index, string hashPrefix, int length)
        {
            string hash;
            int charPosition;
            bool isInteresting;

            do
            {
                hash = doorId.GetMD5Hash(prefix: null, suffix: index.ToString());

                charPosition = char.IsDigit(hash[hashPrefix.Length])
                    ? (int)char.GetNumericValue(hash[hashPrefix.Length])
                    : -1;

                isInteresting = hash.StartsWith(hashPrefix) &&
                    (charPosition >= 0 && charPosition < length);

                index++;
            } while (!isInteresting);

            return (index, hash[hashPrefix.Length + 1], charPosition);
        }

        private string GetHashPrefix(char character, int length)
        {
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                chars[i] = character;
            }
            return new string(chars);
        }
    }
}