using System;
using System.Collections.Generic;
using advent.Input;

namespace advent.Answers._2015
{
    public class _18 : IPuzzleAnswer
    {
        private const int WIDTH = 100;
        private const int HEIGHT = 100;
        private const char LIGHT_ON = '#';
        private const char LIGHT_OFF = '.';

        private readonly IPuzzleInput _input;

        public _18(IPuzzleInput input) => _input = input;

        public string Part1()
        {
            var lights = ParseLights(_input);
            var steps = 100;
            for (var i = 0; i < steps; i++)
            {
                Step(lights);
            }
            var lit = CountLit(lights);
            return $"Number of lights on after {steps} steps: {lit}.";
        }

        public string Part2()
        {
            var lights = ParseLights(_input, cornersStuck: true);
            var steps = 100;
            for (var i = 0; i < steps; i++)
            {
                Step(lights, cornersStuck: true);
            }
            var lit = CountLit(lights);
            return $"Number of lights on after {steps} steps with corners stuck on: {lit}.";
        }

        private void Step(char[,] lights, bool cornersStuck = false)
        {
            // build a list of what changes need to be applied
            // before changing anything
            List<Change> changes = new();
            for (var x = 0; x < WIDTH; x++)
            {
                for (var y = 0; y < HEIGHT; y++)
                {
                    if (ShouldTurnOff(lights, x, y))
                    {
                        changes.Add(new(x, y, LIGHT_OFF));
                    }
                    else if (ShouldTurnOn(lights, x, y))
                    {
                        changes.Add(new(x, y, LIGHT_ON));
                    }
                }
            }

            // apply the changes in the list
            foreach (var change in changes)
            {
                if (cornersStuck && IsCorner(change.X, change.Y))
                {
                    // if the corner is stuck,
                    // it can't change
                    continue;
                }

                lights[change.X, change.Y] = change.Value;
            }
        }

        private bool IsCorner(int x, int y) =>
            (x == 0 && y == 0) ||
            (x == 0 && y == HEIGHT - 1) ||
            (x == WIDTH - 1 && y == 0) ||
            (x == WIDTH - 1 && y == HEIGHT - 1);

        private bool LightExists(int x, int y) =>
            (x >= 0 && x < WIDTH) &&
            (y >= 0 && y < HEIGHT);

        private bool IsOn(char[,] lights, int x, int y) =>
            LightExists(x, y) && lights[x, y] == LIGHT_ON;

        private bool ShouldTurnOff(char[,] lights, int x, int y)
        {
            if (!IsOn(lights, x, y))
            {
                return false;
            }

            return CountLitNeighbors(lights, x, y) switch
            {
                2 or 3 => false,
                _ => true
            };
        }

        private bool ShouldTurnOn(char[,] lights, int x, int y)
        {
            if (IsOn(lights, x, y))
            {
                return false;
            }

            return CountLitNeighbors(lights, x, y) switch
            {
                3 => true,
                _ => false
            };
        }

        private int CountLitNeighbors(char[,] lights, int x, int y)
        {
            int count = 0;

            for (var nX = x - 1; nX <= x + 1; nX++)
            {
                for (var nY = y - 1; nY <= y + 1; nY++)
                {
                    // skip the original light when
                    // counting its neighbors
                    if (nX == x && nY == y)
                    {
                        continue;
                    }

                    if (IsOn(lights, nX, nY))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountLit(char[,] lights)
        {
            var count = 0;

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (IsOn(lights, x, y))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private record Change(int X, int Y, char Value);

        private char[,] ParseLights(IPuzzleInput input, bool cornersStuck = false)
        {
            var lights = new char[WIDTH, HEIGHT];

            var x = 0;
            foreach (ReadOnlySpan<char> line in input.ReadLines())
            {
                for (var y = 0; y < HEIGHT; y++)
                {
                    if (cornersStuck && IsCorner(x, y))
                    {
                        lights[x, y] = LIGHT_ON;
                    }
                    else
                    {
                        lights[x, y] = line[y];
                    }
                }
                x++;
            }

            return lights;
        }
    }
}