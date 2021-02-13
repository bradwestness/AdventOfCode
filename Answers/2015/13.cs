using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2015
{
    public class _13 : IAnswer
    {
        private readonly IDictionary<string, Guest> _guests;

        public _13(string input)
        {
            _guests = ParseGuests(input);
        }

        public string Part1()
        {
            var permutations = _guests.Keys.ToList().GetPermutations();
            var (permutation, totalChange) = GetOptimalPermutation(permutations);

            return $"Total happiness change for optimal arrangement: {totalChange}.";
        }

        public string Part2()
        {
            _guests.Add("Me", new Guest(new Dictionary<string, int>()));
            var permutations = _guests.Keys.ToList().GetPermutations();
            var (permutation, totalChange) = GetOptimalPermutation(permutations);

            return $"Total happiness change for optimal arrangement: {totalChange}.";
        }

        private (IList<string> Permutation, int TotalChange) GetOptimalPermutation(IList<IList<string>> permutations)
        {
            var max = 0;
            var optimal = 0;

            for (var i = 0; i < permutations.Count; i++)
            {
                var change = GetPermutationTotalChange(permutations[i]);
                if (change > max)
                {
                    max = change;
                    optimal = i;
                }
            }

            return (permutations[optimal], max);
        }

        private int GetPermutationTotalChange(IList<string> permutation)
        {
            var change = 0;

            for (var i = 0; i < permutation.Count; i++)
            {
                var name = permutation[i];
                var guest = _guests[name];
                var left = i > 0 
                    ? permutation[i - 1] 
                    : permutation.Last();
                var right = i < permutation.Count - 1 
                    ? permutation[i + 1] 
                    : permutation.First();

                change += guest.Neighbors.ContainsKey(left) ? guest.Neighbors[left] : 0;
                change += guest.Neighbors.ContainsKey(right) ? guest.Neighbors[right] : 0;
            }

            return change;
        }

        private record Guest(IDictionary<string, int> Neighbors);

        private IDictionary<string, Guest> ParseGuests(string input)
        {
            var guests = new Dictionary<string, Guest>();

            foreach (var line in input.ToLines())
            {
                var (name, _, direction, points, _, _, _, _, _, _, neighbor, rest) = line.Trim(' ', '.').Split(' ');

                if (!guests.ContainsKey(name))
                {
                    guests.Add(name, new Guest(new Dictionary<string, int>()));
                }

                var modifier = direction switch
                {
                    "lose" => -1,
                    _ => 1
                };

                if (!guests[name].Neighbors.ContainsKey(neighbor))
                {
                    var number = int.Parse(points) * modifier;
                    guests[name].Neighbors.Add(neighbor, number);
                }
            }

            return guests;
        }
    }
}