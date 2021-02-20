using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2015
{
    public class _22 : IAnswer
    {
        private readonly Enemy _enemy;
        public _22(Input input) => _enemy = ParseEnemy(input);

        public string Part1()
        {
            var hp = 50;
            var mana = 50000;

            var minManaRequired = _spells.Min(s => s.Mana);
            var maxComboItems = (int)mana / minManaRequired;

            // get all spell combinations where the total mana spend
            // is less than the starting mana
            var combos = _spells.GetCombinations(1, 10000)
                .Where(c => c.Sum(s => s.Mana) <= mana);

            Player player = new(hp, mana);
            var minSpend = int.MaxValue;

            foreach (var combo in combos)
            {
                var outcome = PlayGame(player, combo);
                var spend = combo.Sum(s => s.Mana);
                minSpend = outcome switch
                {
                    Outcome.Win when spend < minSpend => spend,
                    _ => minSpend
                };
            }

            return $"Minimum mana spent to win the fight: {minSpend}.";
        }

        public string Part2()
        {
            return $"";
        }

        private Outcome PlayGame(Player player, IList<Spell> spells)
        {
            // start with a fresh enemy every game
            var enemy = _enemy with { };
            var effects = new Dictionary<string, Effect>();
            var minManaRequired = _spells.Min(s => s.Mana);

            while (true)
            {
                // player's turn
                ApplyEffects(ref player, ref enemy, ref effects);
                if (enemy.HitPoints < 1)
                {
                    // effects killed the enemy
                    return Outcome.Win;
                }

                // get the first spell the player can afford
                // that isn't an already active effect
                var spell = spells.FirstOrDefault(s => s.Mana <= player.Mana && !effects.ContainsKey(s.Name));
                if (spell is null)
                {
                    // can't cast a spell, game over
                    return Outcome.Lose;
                }

                // cast the spell
                if (spell.Effect is not null)
                {
                    effects.Add(spell.Name, spell.Effect);
                }
                player = player with
                {
                    Mana = player.Mana - spell.Mana,
                    HitPoints = player.HitPoints + spell.Heal
                };
                enemy = enemy with
                {
                    HitPoints = enemy.HitPoints - spell.Damage
                };
                if (enemy.HitPoints < 1)
                {
                    // spell killed the enemy
                    return Outcome.Win;
                }

                // boss's turn
                ApplyEffects(ref player, ref enemy, ref effects);
                if (enemy.HitPoints < 1)
                {
                    // effects killed the enemy
                    return Outcome.Win;
                }
                var damage = enemy.Damage - effects.Sum(e => e.Value.Armor);
                if (damage < 1)
                {
                    damage = 1;
                }
                player = player with
                {
                    HitPoints = player.HitPoints - damage
                };
                if (player.HitPoints < 1)
                {
                    // enemy killed the player
                    return Outcome.Lose;
                }
            }
        }

        private void ApplyEffects(ref Player player, ref Enemy enemy, ref Dictionary<string, Effect> effects)
        {
            foreach (var key in effects.Keys)
            {
                enemy = enemy with { HitPoints = enemy.HitPoints - effects[key].Damage };
                player = player with { Mana = player.Mana + effects[key].Mana };

                // decrement remaining turns for effect
                effects[key] = effects[key] with
                {
                    Turns = effects[key].Turns - 1
                };

                if (effects[key].Turns < 1)
                {
                    // effect has worn off
                    effects.Remove(key);
                }
            }
        }

        private int CalculateDamage(int characterDamage, int opponentArmor)
        {
            var damage = characterDamage - opponentArmor;
            return new int[] { 1, damage }.Max();
        }

        private Enemy ParseEnemy(Input input)
        {
            Enemy enemy = new(default, default);

            foreach (var line in input.ReadLines())
            {
                var (key, value, _) = line.Split(":");

                enemy = key.Replace(" ", "") switch
                {
                    nameof(Enemy.HitPoints) => enemy with { HitPoints = int.Parse(value) },
                    nameof(Enemy.Damage) => enemy with { Damage = int.Parse(value) },
                    _ => enemy
                };
            }

            return enemy;
        }

        private readonly IList<Spell> _spells = new List<Spell>
        {
            new("Magic Missile", 53, 4, 0, null),
            new("Drain", 73, 2, 2, null),
            // "turns" on the shield effect is one higher
            // because the turn gets subtracted before the shield
            // effect is calculated
            new("Shield", 113, 0, 0, new Effect(7, 0, 0, 0, 7)),
            new("Poison", 173, 0, 0, new Effect(6, 0, 3, 0, 0)),
            new("Recharge", 229, 0, 0, new Effect(5, 101, 0, 0, 0))
        };

        record Effect(int Turns, int Mana, int Damage, int Heal, int Armor);

        record Spell(string Name, int Mana, int Damage, int Heal, Effect Effect);

        record Player(int HitPoints, int Mana);

        record Enemy(int HitPoints, int Damage);

        enum Outcome
        {
            Win,
            Lose,
            Draw
        }
    }
}