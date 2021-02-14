using System;
using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2015
{
    public class _15 : IAnswer
    {
        private readonly IList<Ingredient> _ingredients;

        public _15(Input input)
        {
            _ingredients = ParseIngredients(input);
        }

        public string Part1()
        {
            var cookies = GetAllCookies(100);
            var winner = cookies.OrderByDescending(x => x.Score).First();
            return $"Total score of the winning cookie is {winner.Score}.";
        }

        public string Part2()
        {
            var cookies = GetAllCookies(100);
            var winner = cookies.Where(x => x.Calories == 500).OrderByDescending(x => x.Score).First();
            return $"Total score of the winning cookie is {winner.Score}.";
        }

        private IEnumerable<Cookie> GetAllCookies(int teaspoons)
        {
            var recipes = GetPermutations(new int[_ingredients.Count], teaspoons);
            var cookies = new List<Cookie>();

            foreach (var recipe in recipes)
            {
                var cookie = new Cookie();

                for (var i = 0; i < _ingredients.Count; i++)
                {
                    var tsp = recipe[i];

                    if (tsp > 0)
                    {
                        cookie.AddIngredient(_ingredients[i], tsp);
                    }
                }

                cookies.Add(cookie);
            }

            return cookies;
        }

        public static IEnumerable<int[]> GetPermutations(IEnumerable<int> start, int max, int level = 0)
        {
            var remaining = max - start.Sum();
            var arr = start.ToArray();

            if (level == arr.Length - 1)
            {
                arr[level] = remaining;
                yield return arr;
            }
            else
            {
                for (var i = 0; i < remaining; i++)
                {
                    var copy = start.ToArray();
                    copy[level] = i;

                    foreach (var distribution in GetPermutations(copy, max, level + 1))
                    {
                        yield return distribution;
                    }
                }
            }
        }

        private IList<Ingredient> ParseIngredients(Input input)
        {
            var list = new List<Ingredient>();
            var separators = new[] { ' ', ',', ':' };

            foreach (var line in input.ReadLines())
            {
                var (name, _, capacity, _, durability, _, flavor, _, texture, _, calories, _) = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                list.Add(new Ingredient(
                    name,
                    int.Parse(capacity),
                    int.Parse(durability),
                    int.Parse(flavor),
                    int.Parse(texture),
                    int.Parse(calories)
                ));
            }

            return list;
        }

        private class Cookie
        {
            public int Capacity { get; private set; }
            public int Durability { get; private set; }
            public int Flavor { get; private set; }
            public int Texture { get; private set; }
            public int Calories { get; private set; }
            public int Score =>
                (Capacity > 0 ? Capacity : 0) *
                (Durability > 0 ? Durability : 0) *
                (Flavor > 0 ? Flavor : 0) *
                (Texture > 0 ? Texture : 0);

            public void AddIngredient(Ingredient ingredient, int tsp)
            {
                Capacity += ingredient.Capacity * tsp;
                Durability += ingredient.Durability * tsp;
                Flavor += ingredient.Flavor * tsp;
                Texture += ingredient.Texture * tsp;
                Calories += ingredient.Calories * tsp;
            }
        }

        private class Ingredient
        {
            public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
            {
                Name = name;
                Capacity = capacity;
                Durability = durability;
                Flavor = flavor;
                Texture = texture;
                Calories = calories;
            }

            public string Name { get; private set; }
            public int Capacity { get; private set; }
            public int Durability { get; private set; }
            public int Flavor { get; private set; }
            public int Texture { get; private set; }
            public int Calories { get; private set; }
        }
    }
}