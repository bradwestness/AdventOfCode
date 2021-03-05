using System;
using System.Collections.Generic;
using advent.Input;

namespace advent.Answers._2016
{
    public class _06 : IPuzzleAnswer
    {
        private readonly IPuzzleInput _input;

        public _06(IPuzzleInput input) => _input = input;

        public string Part1()
        {
            var message = GetErrorCorrectedMessage(_input, DecodeByMostCommon);
            return $"Decoded message: {message}";
        }

        public string Part2()
        {
            var message = GetErrorCorrectedMessage(_input, DecodeByLeastCommon);
            return $"Decoded message: {message}";
        }

        private string GetErrorCorrectedMessage(IPuzzleInput input, Func<Dictionary<int, Dictionary<char, int>>, string> decodeStrategy)
        {
            Dictionary<int, Dictionary<char, int>> dict = new();
            char c = default;

            foreach (ReadOnlySpan<char> line in input.ReadLines())
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (!dict.ContainsKey(i))
                    {
                        dict.Add(i, new());
                    }

                    c = line[i];

                    if (!dict[i].ContainsKey(c))
                    {
                        dict[i].Add(c, new());
                    }

                    dict[i][c]++;
                }
            }

            return decodeStrategy(dict);
        }

        private string DecodeByMostCommon(Dictionary<int, Dictionary<char, int>> dict)
        {            
            var chars = new char[dict.Count];

            for (var i = 0; i < chars.Length; i++)
            {
                int max = default;
                char c = default;

                foreach (var key in dict[i].Keys)
                {
                    if (dict[i][key] > max)
                    {
                        c = key;
                        max = dict[i][key];
                    }
                }

                chars[i] = c;
            }

            return new string(chars);
        }

        private string DecodeByLeastCommon(Dictionary<int, Dictionary<char, int>> dict)
        {            
            var chars = new char[dict.Count];            

            for (var i = 0; i < chars.Length; i++)
            {
                int min = int.MaxValue;
                char c = default;

                foreach (var key in dict[i].Keys)
                {
                    if (dict[i][key] < min)
                    {
                        c = key;
                        min = dict[i][key];
                    }
                }

                chars[i] = c;
            }

            return new string(chars);
        }
    }
}