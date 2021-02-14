using System;
using System.Collections.Generic;

namespace advent.Answers._2015
{
    public class _06 : IAnswer
    {
        private readonly Input _input;

        public _06(Input input)
        {
            _input = input;
        }

        public string Part1()
        {
            var lights = new bool[1000, 1000];
            lights.Initialize();

            foreach (var instruction in ParseInstructions(_input))
            {
                for (var row = instruction.Start.Row; row <= instruction.End.Row; row++)
                {
                    for (var col = instruction.Start.Col; col <= instruction.End.Col; col++)
                    {
                        switch (instruction.Action)
                        {
                            case LightAction.Toggle:
                                lights[row, col] = !lights[row, col];
                                break;

                            case LightAction.TurnOff:
                                lights[row, col] = false;
                                break;

                            case LightAction.TurnOn:
                                lights[row, col] = true;
                                break;
                        }
                    }
                }
            }

            return $"Total lights lit: {GetTrueCount(lights)}.";
        }

        public string Part2()
        {
            var lights = new int[1000, 1000];
            lights.Initialize();

            foreach (var instruction in ParseInstructions(_input))
            {
                for (var row = instruction.Start.Row; row <= instruction.End.Row; row++)
                {
                    for (var col = instruction.Start.Col; col <= instruction.End.Col; col++)
                    {
                        switch (instruction.Action)
                        {
                            case LightAction.Toggle:
                                lights[row, col] += 2;
                                break;

                            case LightAction.TurnOff:
                                if (lights[row, col] > 0)
                                {
                                    lights[row, col] -= 1;
                                }
                                break;

                            case LightAction.TurnOn:
                                lights[row, col] += 1;
                                break;
                        }
                    }
                }
            }

            return $"Total brightness of all lights: {GetAggregateTotal(lights)}.";
        }

        private static long GetTrueCount(bool[,] arr)
        {
            var rows = arr.GetLength(0);
            var cols = arr.GetLength(1);
            long count = 0;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    if (arr[row, col])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static long GetAggregateTotal(int[,] arr)
        {
            var rows = arr.GetLength(0);
            var cols = arr.GetLength(1);
            long total = 0;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    total += arr[row, col];
                }
            }

            return total;
        }

        private IEnumerable<Instruction> ParseInstructions(Input input)
        {
            foreach (var line in input.ReadLines())
            {
                var (startToken, endToken, _) = line.Split(" through ");
                var action = ParseAction(startToken);
                var start = ParseStart(startToken);
                var end = ParseEnd(endToken);

                yield return new Instruction(action, start, end);
            }
        }

        private LightAction ParseAction(string line)
        {
            if (line.StartsWith("turn off"))
            {
                return LightAction.TurnOff;
            }

            if (line.StartsWith("turn on"))
            {
                return LightAction.TurnOn;
            }

            if (line.StartsWith("toggle"))
            {
                return LightAction.Toggle;
            }

            throw new Exception($"Unexpected light action: {line}!");
        }

        private Point ParseStart(string start)
        {
            var (rowToken, colToken, _) = start
                .Replace("turn off", "")
                .Replace("turn on", "")
                .Replace("toggle", "")
                .Trim()
                .Split(",");
            int.TryParse(rowToken, out var row);
            int.TryParse(colToken, out var col);
            return new Point(row, col);
        }

        private Point ParseEnd(string end)
        {
            var (rowToken, colToken, _) = end.Trim().Split(",");
            int.TryParse(rowToken, out var row);
            int.TryParse(colToken, out var col);
            return new Point(row, col);
        }

        private record Instruction(LightAction Action, Point Start, Point End);

        private record Point(int Row, int Col);

        private enum LightAction
        {
            Toggle,
            TurnOff,
            TurnOn
        }
    }
}