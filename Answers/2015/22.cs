using System;
using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2015
{
    public class _22 : IAnswer
    {
        private readonly Boss _boss;
        private readonly IList<Spell> _spells = new List<Spell>
        {
            new("Magic Missile", 53, 4, 0, null),
            new("Drain", 73, 2, 2, null),
            new("Shield", 113, 0, 0, new(6, 0, 0, 0, 7)),
            new("Poison", 173, 0, 0, new(6, 0, 3, 0, 0)),
            new("Recharge", 229, 0, 0, new(5, 101, 0, 0, 0))
        };

        public _22(Input input) => _boss = new(input);

        public string Part1()
        {
            Test();

            var minSpent = int.MaxValue;
            IEnumerable<string> history = null;
            PlayGames(ref minSpent, ref history, new(50, 500), _boss.Clone());
            return $"Minimum mana spent to win the fight: {minSpent}.";
        }

        public string Part2()
        {
            var minSpent = int.MaxValue;
            IEnumerable<string> history = null;
            PlayGames(ref minSpent, ref history, new(50, 500, turnHpCost: 1), _boss.Clone());
            return $"Minimum mana spent to win the fight: {minSpent}.";
        }

        private void Test()
        {
            // test 1
            var minSpent = int.MaxValue;
            IEnumerable<string> history = null;
            PlayGames(ref minSpent, ref history, new(10, 250), new(13, 8));
            WriteResults("Test Game 1", history);

            // test 2
            minSpent = int.MaxValue;
            history = null;
            PlayGames(ref minSpent, ref history, new(10, 250), new(14, 8));
            WriteResults("Test Game 2", history);
        }

        private void WriteResults(string heading, IEnumerable<string> history)
        {
            Console.WriteLine($"\n\n{heading}:");
            foreach (var line in history)
            {
                Console.WriteLine($"\t{line}");
            }
            Console.WriteLine("\n\n");
        }

        // Recursively play every possible spell that the player can afford
        // as long as the player hasn't died, and the amount spent is less
        // than the minimum winning amount
        private void PlayGames(ref int minSpent, ref IEnumerable<string> history, Player player, Boss boss)
        {
            foreach (var spell in _spells)
            {
                if (spell.Mana > player.Mana || player.ActiveEffects.Any(y => y.Key == spell.Name))
                {
                    // either the player can't afford this spell,
                    // or its an effect that is already active
                    continue;
                }

                var newPlayer = player.Clone();
                var newBoss = boss.Clone();

                newPlayer.TakeTurn(newBoss, spell);
                if (!newPlayer.IsDefeated & !newBoss.IsDefeated)
                {
                    newBoss.TakeTurn(newPlayer);
                }

                if (newPlayer.Spent < minSpent)
                {
                    if (newBoss.IsDefeated)
                    {
                        // the player won, so update the minimum spent and winning history
                        minSpent = newPlayer.Spent;
                        history = newPlayer.History;
                    }
                    else if (!newPlayer.IsDefeated)
                    {
                        // if both characters are still alive,
                        // and we haven't outspent the minimum,                    
                        // we need to keep following this branch
                        PlayGames(ref minSpent, ref history, newPlayer, newBoss);
                    }
                }
            }
        }

        record Effect(int Turns, int Mana, int Damage, int Heal, int Armor);

        record Spell(string Name, int Mana, int Damage, int Heal, Effect Effect);

        class Player
        {
            private readonly IList<string> _history = new List<string>();
            private readonly IDictionary<string, Effect> _effects = new Dictionary<string, Effect>();

            public Player(int hitPoints, int mana, int turnHpCost = 0)
            {
                HitPoints = hitPoints;
                Mana = mana;
                TurnHpCost = turnHpCost;
            }

            public int HitPoints { get; private set; }
            public int Mana { get; private set; }
            public int Spent { get; private set; }
            public int TurnHpCost { get; private set; }

            public int Armor =>
                ActiveEffects.Sum(e => e.Value.Armor);

            public IEnumerable<KeyValuePair<string, Effect>> ActiveEffects =>
                _effects.Where(e => e.Value.Turns > 0);

            public bool IsDefeated =>
                HitPoints <= TurnHpCost;

            public IEnumerable<string> History => _history;

            public void AddHistory(string item) => _history.Add(item);

            public void TakeTurn(Boss boss, Spell spell)
            {
                if (IsDefeated || boss.IsDefeated)
                {
                    return;
                }

                HitPoints -= TurnHpCost;
                AddHistory("");
                AddHistory($"-- Player turn --");
                AddHistory($"- Player has {HitPoints} hit points, {Armor} armor, {Mana} mana");
                AddHistory($"- Boss has {boss.HitPoints} hit points");
                if (TurnHpCost > 0)
                {
                    AddHistory($"Turn costs player {TurnHpCost} hit points; {HitPoints} hit points remaining.");
                }
                ApplyEffects(boss);
                CastSpell(boss, spell);
            }

            private void CastSpell(Boss boss, Spell spell)
            {
                if (IsDefeated || boss.IsDefeated)
                {
                    return;
                }

                Mana -= spell.Mana;
                Spent += spell.Mana;
                AddHistory($"Player casts {spell.Name} for {spell.Mana} mana; {Mana} mana remaining.");

                if (spell.Heal > 0)
                {
                    HitPoints += spell.Heal;
                    AddHistory($"{spell.Name} recovers {spell.Heal} hit points; {HitPoints} hit points remaining.");
                }

                if (spell.Damage > 0)
                {
                    boss.Defend(spell.Damage);
                    AddHistory($"{spell.Name} deals {spell.Damage} damage; boss has {boss.HitPoints} hit points remaining.");
                    if (boss.IsDefeated)
                    {
                        AddHistory($"This kills the boss, and the player wins.");
                        return;
                    }
                }

                if (spell.Effect?.Armor > 0)
                {
                    AddHistory($"{spell.Name} increases armor by {spell.Effect.Armor}.");
                }

                if (spell.Effect is not null)
                {
                    _effects.Add(spell.Name, spell.Effect);
                }
            }

            public void Defend(Boss boss)
            {
                if (IsDefeated || boss.IsDefeated)
                {
                    return;
                }

                var armor = Armor;
                var hp = Math.Max(1, boss.Damage - armor);
                HitPoints -= hp;
                AddHistory($"Boss attacks for {boss.Damage} - {armor} = {hp} damage; {HitPoints} hit points remaining.");
                if (IsDefeated)
                {
                    AddHistory($"This kills the player! Game over.");
                }
            }

            public void ApplyEffects(Boss boss)
            {
                if (IsDefeated || boss.IsDefeated)
                {
                    return;
                }

                foreach (var key in _effects.Keys)
                {
                    if (_effects[key].Damage > 0)
                    {
                        boss.Defend(_effects[key].Damage);
                        AddHistory($"{key} deals {_effects[key].Damage} damage; boss has {boss.HitPoints} hit points remaining.");
                        if (boss.IsDefeated)
                        {
                            AddHistory($"This kills the boss, and the player wins.");
                            return;
                        }
                    }

                    if (_effects[key].Mana > 0)
                    {
                        Mana += _effects[key].Mana;
                        AddHistory($"{key} provides {_effects[key].Mana} mana; {Mana} mana remaining.");
                    }

                    _effects[key] = _effects[key] with
                    {
                        Turns = _effects[key].Turns - 1
                    };

                    if (_effects[key].Turns < 1)
                    {
                        _effects.Remove(key);
                        AddHistory($"{key} wears off.");
                    }
                    else
                    {
                        AddHistory($"{key}'s timer is now {_effects[key].Turns}.");
                    }
                }
            }

            public Player Clone()
            {
                Player clone = new(HitPoints, Mana, TurnHpCost);
                clone.Spent = Spent;

                foreach (var item in _history)
                {
                    clone.AddHistory(item);
                }

                foreach (var key in _effects.Keys)
                {
                    clone._effects.Add(key, _effects[key] with { });
                }

                return clone;
            }
        }

        class Boss
        {
            public Boss()
            {

            }

            public Boss(int hitPoints, int damage)
            {
                HitPoints = hitPoints;
                Damage = damage;
            }

            public Boss(Input input)
            {
                foreach (var line in input.ReadLines())
                {
                    var (key, value, _) = line.Split(":");

                    switch (key.Replace(" ", ""))
                    {
                        case nameof(HitPoints):
                            HitPoints = int.Parse(value);
                            break;

                        case nameof(Damage):
                            Damage = int.Parse(value);
                            break;
                    }
                }
            }

            public int HitPoints { get; private set; }
            public int Damage { get; private set; }

            public bool IsDefeated =>
                HitPoints < 1;

            public void TakeTurn(Player player)
            {
                if (IsDefeated || player.IsDefeated)
                {
                    return;
                }

                player.AddHistory("");
                player.AddHistory($"-- Boss turn --");
                player.AddHistory($"- Player has {player.HitPoints} hit points, {player.Armor} armor, {player.Mana} mana");
                player.AddHistory($"- Boss has {HitPoints} hit points");
                player.ApplyEffects(this);
                player.Defend(this);
            }

            public void Defend(int damage)
            {
                if (IsDefeated)
                {
                    return;
                }

                HitPoints -= damage;
            }

            public Boss Clone() => new Boss
            {
                HitPoints = HitPoints,
                Damage = Damage
            };
        }
    }
}