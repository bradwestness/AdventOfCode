namespace advent._2015
{
    public class _04 : IAnswer
    {
        public string Part1()
        {
            const string startsWith = "00000";
            long i = 0;
            string hash = i.ToString().GetMD5Hash(INPUT);

            while (!hash.StartsWith(startsWith))
            {
                i++;
                hash = i.ToString().GetMD5Hash(INPUT);
            }

            return $"First integer with a hash that starts with {startsWith}: {i}.";
        }
        
        public string Part2()
        {
            const string startsWith = "000000";
            long i = 0;
            string hash = i.ToString().GetMD5Hash(INPUT);

            while (!hash.StartsWith(startsWith))
            {
                i++;
                hash = i.ToString().GetMD5Hash(INPUT);
            }

            return $"First integer with a hash that starts with {startsWith}: {i}.";
        }

        private const string INPUT = @"iwrupvqb";
    }
}
