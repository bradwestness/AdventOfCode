using System.Collections.Generic;
using System.Linq;
using advent.Input;

namespace advent.Answers._2015
{
    public class _21 : IPuzzleAnswer
    {
        private readonly Character _enemy;
        public _21(IPuzzleInput input) => _enemy = ParseEnemy(input);

        public string Part1()
        {
            var (minWinningCost, _) = PlayAllCombinations();
            return $"Minimum winning cost: {minWinningCost}.";
        }

        public string Part2()
        {
            var (_, maxLosingCost) = PlayAllCombinations();
            return $"Maximum losing cost: {maxLosingCost}.";
        }

        private (int MinWinningCost, int MaxLosingCost) PlayAllCombinations()
        {
            int minWinningCost = int.MaxValue;
            int maxLosingCost = 0;

            foreach (var items in GetItemCombinations())
            {
                var cost = items.Sum(i => i.Cost);
                var player = new Character
                {
                    HitPoints = 100,
                    Armor = items.Sum(i => i.Armor),
                    Damage = items.Sum(i => i.Damage)
                };

                switch (PlayGame(player))
                {
                    case Outcome.Win when cost < minWinningCost:
                        minWinningCost = cost;
                        break;

                    case Outcome.Lose when cost > maxLosingCost:
                        maxLosingCost = cost;
                        break;
                }
            }

            return (minWinningCost, maxLosingCost);
        }

        private IEnumerable<IEnumerable<Item>> GetItemCombinations()
        {
            var weapons = _shop.Weapons.GetCombinations(minItems: 1, maxItems: 1);
            var armor = _shop.Armor.GetCombinations(minItems: 0, maxItems: 1);
            var rings = _shop.Rings.GetCombinations(minItems: 0, maxItems: 2);

            foreach (var w in weapons)
            {
                foreach (var a in armor)
                {
                    foreach (var r in rings)
                    {
                        yield return w.Concat(a).Concat(r);
                    }
                }
            }
        }

        private Outcome PlayGame(Character player)
        {
            // start with a fresh enemy every game
            var enemy = _enemy.Clone();

            while (player.HitPoints > 0 && enemy.HitPoints > 0)
            {
                // player goes first
                enemy.HitPoints -= CalculateDamage(player.Damage, enemy.Armor);
                if (enemy.HitPoints < 1)
                {
                    return Outcome.Win;
                }

                // enemy goes next
                player.HitPoints -= CalculateDamage(enemy.Damage, player.Armor);
                if (player.HitPoints < 1)
                {
                    return Outcome.Lose;
                }
            }

            return Outcome.Draw;
        }

        private int CalculateDamage(int characterDamage, int opponentArmor)
        {
            var damage = characterDamage - opponentArmor;

            return damage > 1
                ? damage
                : 1;
        }

        private Character ParseEnemy(IPuzzleInput input)
        {
            Character enemy = new();
            var type = enemy.GetType();

            foreach (var line in input.ReadLines())
            {
                var (key, value, _) = line.Split(":");
                var property = type.GetProperty(key.Replace(" ", ""));

                if (property != null)
                {
                    property.SetValue(enemy, int.Parse(value));
                }
            }

            return enemy;
        }

        enum Outcome
        {
            Win,
            Lose,
            Draw
        }

        class Character
        {
            public int HitPoints { get; set; }
            public int Damage { get; set; }
            public int Armor { get; set; }

            public Character Clone() => MemberwiseClone() as Character;
        }

        record Item(string Name, int Cost, int Damage, int Armor);

        record Shop(IList<Item> Weapons, IList<Item> Armor, IList<Item> Rings);

        private readonly Shop _shop = new(
            new List<Item> // weapons
            {
                new("Dagger", 8, 4, 0),
                new("Shortsword", 10, 5, 0),
                new("Warhammer", 25, 6, 0),
                new("Longsword", 40, 7, 0),
                new("Greataxe", 74, 8, 0)
            },
            new List<Item> // armor
            {
                new("Leather", 13, 0, 1),
                new("Chainmail", 31, 0, 2),
                new("Splintmail", 53, 0, 3),
                new("Bandedmail", 75, 0, 4),
                new("Platemail", 102, 0, 5)
            },
            new List<Item> // rings
            {
                new("Damage +1", 25, 1, 0),
                new("Damage +2", 50, 2, 0),
                new("Damage +3", 100, 3, 0),
                new("Defense +1", 20, 0, 1),
                new("Defense +2", 40, 0, 2),
                new("Defense +3", 80, 0, 3)
            }
        );
    }
}