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
            var count = GetCombinationCount(liters);
            return $"Total combinations adding up to {liters}: {count}.";
        }

        public string Part2()
        {
            var liters = 150;
            var containers = GetMinContainers(liters);
            var count = GetCombinationCount(liters, minItems: containers, maxItems: containers);
            return $"Total combinations adding up to {liters} using exactly {containers} containers: {count}.";
        }

        private int GetMinContainers(int liters)
        {
            var min = int.MaxValue;

            foreach (var combination in _containers.GetCombinations())
            {
                if (combination.Sum() == liters &&
                    combination.Count < min)
                {
                    min = combination.Count;
                }
            }

            return min;
        }

        private int GetCombinationCount(int liters, int minItems = 1, int? maxItems = null)
        {
            var count = 0;

            foreach (var combination in _containers.GetCombinations(minItems, maxItems))
            {
                if (combination.Sum() == liters)
                {
                    count++;
                }
            }

            return count;
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