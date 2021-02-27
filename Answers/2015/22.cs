using System;
using System.Collections.Generic;

namespace advent.Answers._2015
{
    public class _22 : IAnswer
    {
        private readonly Boss _boss;

        public _22(Input input) => _boss = new(input);

        public string Part1()
        {
            var minSpent = int.MaxValue;
            var player = new Player(50, 500);
            var boss = _boss.Clone();

            PlayGames(ref minSpent, player, boss);
            return $"Minimum mana spent to win the fight: {minSpent}.";
        }

        public string Part2()
        {
            var minSpent = int.MaxValue;
            var player = new Player(50, 500, 1);
            var boss = _boss.Clone();

            PlayGames(ref minSpent, player, boss);
            return $"Minimum mana spent to win the fight: {minSpent}.";
        }

        private void PlayGames(ref int minSpent, Player player, Boss boss)
        {
            foreach (var spellName in _spells.Keys)
            {
                if (!player.CanCast(spellName))
                {
                    continue;
                }

                var newPlayer = player.Clone();
                var newBoss = boss.Clone();

                newPlayer.TakeTurn(newBoss, spellName);
                newBoss.TakeTurn(newPlayer);

                if (newBoss.IsDefeated && newPlayer.Spent < minSpent)
                {
                    minSpent = newPlayer.Spent;
                }

                if (!newPlayer.IsDefeated && !newBoss.IsDefeated && newPlayer.Spent < minSpent)
                {
                    PlayGames(ref minSpent, newPlayer, newBoss);
                }
            }
        }

        private class Player
        {
            public Player(int hitPoints, int mana, int turnHpCost = 0)
            {
                HitPoints = hitPoints;
                Mana = mana;
                TurnHpCost = turnHpCost;

                foreach (var spellName in _spells.Keys)
                {
                    Effects.Add(spellName, 0);
                }
            }

            public int HitPoints { get; set; }
            public int Mana { get; set; }
            public int TurnHpCost { get; set; }
            public int Armor { get; set; }
            public int Spent { get; set; }
            private readonly IDictionary<string, int> Effects = new Dictionary<string, int>();

            public bool IsDefeated =>
                HitPoints <= TurnHpCost;

            public bool CanCast(string spellName) =>
                Effects[spellName] <= 1 &&
                _spells[spellName].Cost <= Mana;

            public void TakeTurn(Boss boss, string spellName)
            {
                var spell = _spells[spellName];

                HitPoints -= TurnHpCost;

                if (IsDefeated)
                {
                    return;
                }

                ApplyEffects(boss);

                if (boss.IsDefeated)
                {
                    return;
                }

                Mana -= spell.Cost;
                Spent += spell.Cost;
                HitPoints += spell.Heal;
                Effects[spellName] += spell.Turns;
                boss.HitPoints -= spell.Damage;

                if (spell.Start is object)
                {
                    spell.Start(this);
                }
            }

            public void ApplyEffects(Boss boss)
            {
                foreach (var spellName in Effects.Keys)
                {
                    if (Effects[spellName] < 1)
                    {
                        continue;
                    }

                    var spell = _spells[spellName];

                    if (spell.Effect is object)
                    {
                        spell.Effect(this, boss);

                        if (boss.IsDefeated)
                        {
                            return;
                        }
                    }

                    Effects[spellName]--;

                    if (Effects[spellName] == 0 && spell.End is object)
                    {
                        spell.End(this);
                    }
                }
            }

            public Player Clone()
            {
                var clone = new Player(HitPoints, Mana, TurnHpCost);
                clone.Spent = Spent;
                clone.Armor = Armor;

                foreach (var spellName in Effects.Keys)
                {
                    clone.Effects[spellName] = Effects[spellName];
                }

                return clone;
            }
        }

        private class Boss
        {
            public Boss()
            {

            }

            public Boss(Input input)
            {
                foreach (var line in input.ReadLines())
                {
                    var (property, value, _) = line.Split(": ");
                    int.TryParse(value, out var intVal);

                    switch (property.Replace(" ", ""))
                    {
                        case nameof(HitPoints):
                            HitPoints = intVal;
                            break;

                        case nameof(Damage):
                            Damage = intVal;
                            break;
                    }
                }
            }

            public int HitPoints { get; set; }
            public int Damage { get; set; }

            public bool IsDefeated =>
                HitPoints <= 0;

            public void TakeTurn(Player player)
            {
                player.ApplyEffects(this);

                if (IsDefeated)
                {
                    return;
                }

                var damage = Math.Max(1, Damage - player.Armor);
                player.HitPoints -= damage;
            }

            public Boss Clone()
            {
                var clone = new Boss();
                clone.HitPoints = HitPoints;
                clone.Damage = Damage;

                return clone;
            }
        }

        private record Spell(
            int Cost,
            int Damage,
            int Heal,
            int Turns,
            Action<Player> Start,
            Action<Player, Boss> Effect,
            Action<Player> End
        );

        private static readonly IDictionary<string, Spell> _spells = new Dictionary<string, Spell>
        {
            { "Magic Missile", new(53, 4, 0, 0, null, null, null) },
            { "Drain", new(73, 2, 2, 0, null, null, null) },
            { "Shield", new(113, 0, 0, 6, p => p.Armor += 7, null, p => p.Armor -= 7) },
            { "Poison", new(173, 0, 0, 6, null, (_, b) => b.HitPoints -= 3, null) },
            { "Recharge", new(229, 0, 0, 5, null, (p, _) => p.Mana += 101, null) }
        };
    }
}