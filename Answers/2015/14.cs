using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace advent.Answers._2015
{
    public class _14 : IAnswer
    {
        private readonly Reindeer[] _reindeer;

        public _14(string input)
        {
            _reindeer = ParseReindeer(input);
        }

        public string Part1()
        {
            Race(2503);
            var winner = _reindeer.OrderByDescending(x => x.Distance).First();
            return $"Winning reindeer was {winner.Name}, who traveled {winner.Distance} kilometers.";
        }

        public string Part2()
        {
            Race(2503);
            var winner = _reindeer.OrderByDescending(x => x.Points).First();
            return $"Winning reindeer was {winner.Name}, who scored {winner.Points} points.";
        }

        private void Race(int length)
        {
            // reset everyone before starting the race
            foreach (var reindeer in _reindeer)
            {
                reindeer.Reset();
            }

            // run the race
            for (var second = 1; second <= length; second++)
            {
                // move the reindeer
                foreach (var reindeer in _reindeer)
                {
                    reindeer.Move();
                }

                // award a point to the leader(s)
                var maxDistance = _reindeer.Max(r => r.Distance);
                foreach (var reindeer in _reindeer)
                {
                    if (reindeer.Distance == maxDistance)
                    {
                        reindeer.AwardPoint();
                    }
                }
            }
        }

        private Reindeer[] ParseReindeer(string input)
        {
            var list = new List<Reindeer>();
            int i = 0;

            foreach (var line in input.ToLines())
            {
                var (name, _, _, speed, _, _, flySeconds, _, _, _, _, _, _, restSeconds, _) = line.Split(' ');
                list.Add(new Reindeer(name, int.Parse(speed), int.Parse(flySeconds), int.Parse(restSeconds)));
                i++;
            }

            return list.ToArray();
        }

        private class Reindeer
        {
            public Reindeer(string name, int speed, int flySeconds, int restSeconds)
            {
                Name = name;
                Speed = speed;
                FlySeconds = flySeconds;
                RestSeconds = restSeconds;
            }

            public string Name { get; private set; } = string.Empty;
            public int Speed { get; private set; } = 0;
            public int FlySeconds { get; private set; } = 0;
            public int RestSeconds { get; private set; } = 0;
            public int Flying { get; private set; } = 0;
            public int Resting { get; private set; } = 0;
            public int Distance { get; private set; } = 0;
            public int Points { get; private set; } = 0;

            public void Move()
            {
                if (Flying < FlySeconds)
                {
                    Flying++;
                    Distance += Speed;
                }
                else if (Resting < RestSeconds)
                {
                    Resting++;
                }
                else
                {
                    // just finished resting, so start flying again
                    Resting = 0;
                    Flying = 1;
                    Distance += Speed;
                }
            }

            public void AwardPoint()
            {
                Points++;
            }

            public void Reset()
            {
                Flying = 0;
                Resting = 0;
                Distance = 0;
                Points = 0;
            }
        }
    }
}