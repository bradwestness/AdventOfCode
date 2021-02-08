﻿namespace advent.Answers._2015
{
    public class _04 : IAnswer
    {
        private readonly string _input;

        public _04(string input)
        {
            _input = input;
        }

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
