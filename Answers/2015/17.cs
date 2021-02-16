using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2015
{
    public class _17 : IAnswer
    {
        private readonly IList<int> _containers;

        public _17(Input input) => _containers = ParseContainers(input);

        public string Part1()
        {
            var liters = 150;
            var counts = GetCombinationCounts(liters);
            var total = counts.Sum(item => item.Value);
            return $"Total combinations adding up to {liters}: {total}.";
        }

        public string Part2()
        {
            var liters = 150;
            var counts = GetCombinationCounts(liters);
            var min = counts.Keys.Min();
            var total = counts[min];
            return $"Total combinations adding up to {liters} using exactly {min} containers: {total}.";
        }

        // Gets a dictionary of counts for how many combinations of containers
        // added up exactly to the specified number of liters.
        // Example: if the resulting dictionary has a key of 5 with a value of 20,
        // there were 20 combinations which each used 5 containers to add up to
        // the specified number of liters.
        private IDictionary<int, int> GetCombinationCounts(int liters)
        {
            Dictionary<int, int> counts = new();

            foreach (var combination in _containers.GetCombinations())
            {
                if (combination.Sum() == liters)
                {
                    if (!counts.ContainsKey(combination.Count))
                    {
                        counts.Add(combination.Count, 0);
                    }

                    counts[combination.Count]++;
                }
            }

            return counts;
        }

        private IList<int> ParseContainers(Input input)
        {
            List<int> containers = new();

            foreach (var line in input.ReadLines())
            {
                containers.Add(int.Parse(line));
            }

            return containers;
        }
    }
}