using System;
using System.Collections.Generic;

namespace advent.Answers._2015
{
    public class _03 : IAnswer
    {
        private readonly string _input;

        public _03(string input)
        {
            _input = input;
        }

        public string Part1()
        {
            var presents = new Dictionary<string, int>();
            ReadOnlySpan<char> chars = _input;
            int x = 0;
            int y = 0;

            GivePresent(ref presents, x, y);

            for (var pos = 0; pos < chars.Length; pos++)
            {
                switch (chars[pos])
                {
                    case '^':
                        y += 1;
                        break;

                    case '>':
                        x += 1;
                        break;

                    case 'v':
                        y -= 1;
                        break;

                    case '<':
                        x -= 1;
                        break;
                }

                GivePresent(ref presents, x, y);
            }

            var totalHousesWithPresents = presents.Count;

            return $"Total houses with presents: {totalHousesWithPresents}.";
        }

        public string Part2()
        {
            var presents = new Dictionary<string, int>();
            ReadOnlySpan<char> chars = _input;
            int santaX = 0;
            int santaY = 0;
            int roboX = 0;
            int roboY = 0;
            bool isRobo = false;

            GivePresent(ref presents, santaX, santaY);
            GivePresent(ref presents, roboX, roboY);

            for (var pos = 0; pos < chars.Length; pos++)
            {
                switch (chars[pos])
                {
                    case '^':
                        if (isRobo)
                        {
                            roboY += 1;
                        }
                        else
                        {
                            santaY += 1;
                        }
                        break;

                    case '>':
                        if (isRobo)
                        {
                            roboX += 1;
                        }
                        else
                        {
                            santaX += 1;
                        }
                        break;

                    case 'v':
                        if (isRobo)
                        {
                            roboY -= 1;
                        }
                        else
                        {
                            santaY -= 1;
                        }
                        break;

                    case '<':
                        if (isRobo)
                        {
                            roboX -= 1;
                        }
                        else
                        {
                            santaX -= 1;
                        }
                        break;
                }

                if (isRobo)
                {
                    GivePresent(ref presents, roboX, roboY);
                }
                else
                {
                    GivePresent(ref presents, santaX, santaY);
                }

                isRobo = !isRobo;
            }

            var totalHousesWithPresents = presents.Count;

            return $"Total houses with presents: {totalHousesWithPresents}.";
        }

        private static void GivePresent(ref Dictionary<string, int> presents, int x, int y)
        {
            var key = $"{x},{y}";

            if (!presents.ContainsKey(key))
            {
                presents.Add(key, 0);
            }

            presents[key] += 1;
        }
    }
}
