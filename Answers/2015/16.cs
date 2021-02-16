using System.Collections.Generic;

namespace advent.Answers._2015
{
    public class _16 : IAnswer
    {
        private IDictionary<int, IDictionary<string, int>> _aunts;

        public _16(Input input)
        {
            _aunts = ParseAunts(input);
        }

        public string Part1()
        {
            var sue = FindMatch(_tickerTape, ranges: false);
            return $"The matching Aunt Sue is # {sue}.";
        }

        public string Part2()
        {
            var sue = FindMatch(_tickerTape, ranges: true);
            return $"The matching Aunt Sue is # {sue}.";
        }

        private IDictionary<string, int> _tickerTape => new Dictionary<string, int>
        {
            { "children", 3 },
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 }
        };

        private bool IsMatch(int aunt, string key, int value, bool ranges)
        {
            if (!_aunts[aunt].ContainsKey(key))
            {
                return false;
            }

            return key switch
            {
                "cats" or "trees" when ranges => _aunts[aunt][key] > value,
                "pomeranians" or "goldfish" when ranges => _aunts[aunt][key] < value,
                _ => _aunts[aunt][key] == value
            };
        }

        private int FindMatch(IDictionary<string, int> input, bool ranges)
        {
            var max = -1;
            var sue = -1;

            foreach (var aunt in _aunts.Keys)
            {
                var matches = 0;

                foreach (var key in input.Keys)
                {
                    if (IsMatch(aunt, key, input[key], ranges))
                    {
                        matches++;
                        if (matches > max)
                        {
                            max = matches;
                            sue = aunt;
                        }
                    }
                }
            }

            return sue;
        }

        private IDictionary<int, IDictionary<string, int>> ParseAunts(Input input)
        {
            Dictionary<int, IDictionary<string, int>> dict = new();

            foreach (var line in input.ReadLines())
            {
                var keyValuePairs = line.Split(",");
                var aunt = -1;

                for (var i = 0; i < keyValuePairs.Length; i++)
                {
                    var kvp = keyValuePairs[i];

                    if (i == 0)
                    {
                        var firstColon = kvp.IndexOf(':');
                        aunt = int.Parse(kvp.Substring(3, firstColon - 3));
                        kvp = kvp.Substring(firstColon + 1);
                        dict.Add(aunt, new Dictionary<string, int>());
                    }

                    var (key, value, _) = kvp.Split(':');
                    dict[aunt].Add(key.Trim(), int.Parse(value));
                }
            }

            return dict;
        }
    }
}