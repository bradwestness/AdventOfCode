using System.Collections.Generic;
using System.Linq;
using advent.Input;

namespace advent.Answers._2015
{
    public class _24 : IPuzzleAnswer
    {
        private readonly ulong[] _packages;

        public _24(IPuzzleInput input)
        {
            var packages = new List<ulong>();

            foreach (var line in input.ReadLines())
            {
                if (ushort.TryParse(line, out var package))
                {
                    packages.Add(package);
                }
            }

            _packages = packages.OrderByDescending(p => p).ToArray();
        }

        public string Part1()
        {
            var groupWeight = Sum(_packages) / 3;
            var minEntanglement = GetMinEntanglement(groupWeight);
            return $"Lowest possible entanglement for group 1: {minEntanglement}.";
        }

        public string Part2()
        {
            var groupWeight = Sum(_packages) / 4;
            var minEntanglement = GetMinEntanglement(groupWeight);
            return $"Lowest possible entanglement for group 1: {minEntanglement}.";
        }

        private ulong GetMinEntanglement(ulong groupWeight)
        {
            var minEntanglement = ulong.MaxValue;

            for (var groupSize = 1; minEntanglement == ulong.MaxValue; groupSize++)
            {
                foreach (var group in _packages.GetCombinations(++groupSize, groupSize))
                {
                    if (Sum(group) == groupWeight)
                    {
                        var entanglement = Product(group);

                        if (entanglement < minEntanglement)
                        {
                            minEntanglement = entanglement;
                        }
                    }
                }
            }

            return minEntanglement;
        }

        private ulong Sum(IEnumerable<ulong> enumerable)
        {
            ulong sum = 0;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            return sum;
        }

        private ulong Product(IEnumerable<ulong> enumerable) =>
            enumerable.Aggregate((a, b) => a * b);
    }
}