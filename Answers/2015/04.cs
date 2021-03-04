using advent.Input;

namespace advent.Answers._2015
{
    public class _04 : IPuzzleAnswer
    {
        private readonly string _input;

        public _04(IPuzzleInput input) => _input = input.ReadToEnd();

        public string Part1()
        {
            const string startsWith = "00000";
            long i = 0;
            string hash = i.ToString().GetMD5Hash(_input);

            while (!hash.StartsWith(startsWith))
            {
                i++;
                hash = i.ToString().GetMD5Hash(_input);
            }

            return $"First integer with a hash that starts with {startsWith}: {i}.";
        }

        public string Part2()
        {
            const string startsWith = "000000";
            long i = 0;
            string hash = i.ToString().GetMD5Hash(_input);

            while (!hash.StartsWith(startsWith))
            {
                i++;
                hash = i.ToString().GetMD5Hash(_input);
            }

            return $"First integer with a hash that starts with {startsWith}: {i}.";
        }
    }
}
