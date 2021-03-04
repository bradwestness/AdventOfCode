using System.Collections.Generic;
using System.Linq;
using advent.Input;

namespace advent.Answers._2015
{
    public class _09 : IPuzzleAnswer
    {
        private readonly IPuzzleInput _input;

        public _09(IPuzzleInput input) => _input = input;

        public string Part1()
        {
            var shortest = Routes.Min(x => x.Total);
            return $"Shortest route: {shortest}.";
        }

        public string Part2()
        {
            var longest = Routes.Max(x => x.Total);
            return $"Longest route: {longest}.";
        }

        private IEnumerable<Route> _routes;
        private IEnumerable<Route> Routes =>
            _routes ?? (_routes = GetAllRoutes(_input));

        private record Leg(string From, string To, int Distance);

        private record Route(IEnumerable<Leg> Legs, int Total);

        private static IEnumerable<Route> GetAllRoutes(IPuzzleInput input)
        {
            var cities = ParseCities(input);
            var cityNames = cities.Keys.Distinct().OrderBy(x => x).ToList();
            var permutations = cityNames.GetPermutations();
            var routes = new List<Route>();

            for (var p = 0; p < permutations.Count; p++)
            {
                var permutation = permutations[p];
                var legs = new List<Leg>();
                int total = 0;

                for (var c = 0; c < permutation.Count - 1; c++)
                {
                    var startCityName = permutation[c];
                    var stopCityName = permutation[c + 1];
                    var startCity = cities[startCityName];
                    var distance = startCity[stopCityName];

                    legs.Add(new Leg(startCityName, stopCityName, distance));
                    total += distance;
                }

                routes.Add(new Route(legs, total));
            }

            return routes;
        }

        private static IDictionary<string, IDictionary<string, int>> ParseCities(IPuzzleInput input)
        {
            var cities = new Dictionary<string, IDictionary<string, int>>();

            foreach (var line in input.ReadLines())
            {
                var (start, rest, _) = line.Split(" to ");
                var (stop, distanceStr, _) = rest.Split(" = ");
                int.TryParse(distanceStr.Trim(), out var distance);

                start = start.Trim();
                stop = stop.Trim();

                if (!cities.ContainsKey(start))
                {
                    cities.Add(start, new Dictionary<string, int>());
                }
                cities[start].Add(stop, distance);

                if (!cities.ContainsKey(stop))
                {
                    cities.Add(stop, new Dictionary<string, int>());
                }
                cities[stop].Add(start, distance);
            }

            return cities;
        }
    }
}