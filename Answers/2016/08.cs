using System;
using System.Collections.Generic;
using System.Text;
using advent.Input;

namespace advent.Answers._2016
{
    public class _08 : IPuzzleAnswer
    {
        private readonly IPuzzleInput _input;

        public _08(IPuzzleInput input) => _input = input;

        public string Part1()
        {
            var screen = new bool[50, 6];
            ProcessInstructions(screen, _input.ReadLines());

            var count = CountLitPixels(screen);
            return $"Total lit pixels: {count}.";
        }

        public string Part2()
        {
            var screen = new bool[50, 6];
            ProcessInstructions(screen, _input.ReadLines());

            var message = GetMessage(screen, letterWidth: 5);
            return $"Message being displayed:\n{message}";
        }

        private void ProcessInstructions(bool[,] screen, IEnumerable<string> instructions)
        {
            foreach (var line in instructions)
            {
                var (first, second, third, _, fifth, _) = line.Split(' ');

                switch (first)
                {
                    case "rect":
                        var (a, b, _) = second.Split('x');
                        TurnOnRect(screen, int.Parse(a), int.Parse(b));
                        break;

                    case "rotate" when second == "row":
                        var (_, row, _) = third.Split('=');
                        RotateRow(screen, int.Parse(row), int.Parse(fifth));
                        break;

                    case "rotate" when second == "column":
                        var (_, col, _) = third.Split('=');
                        RotateColumn(screen, int.Parse(col), int.Parse(fifth));
                        break;
                }
            }
        }

        private void TurnOnRect(bool[,] screen, int columns, int rows)
        {
            for (var column = 0; column < columns; column++)
            {
                for (var row = 0; row < rows; row++)
                {
                    screen[column, row] = true;
                }
            }
        }

        private void RotateRow(bool[,] screen, int row, int amount)
        {
            int columns = screen.GetLength(0);

            for (var i = 0; i < amount; i++)
            {
                var last = screen[columns - 1, row];

                for (var column = columns - 1; column > 0; column--)
                {
                    screen[column, row] = screen[column - 1, row];
                }

                screen[0, row] = last;
            }
        }

        private void RotateColumn(bool[,] screen, int column, int amount)
        {
            int rows = screen.GetLength(1);

            for (var i = 0; i < amount; i++)
            {
                var last = screen[column, rows - 1];

                for (var row = rows - 1; row > 0; row--)
                {
                    screen[column, row] = screen[column, row - 1];
                }

                screen[column, 0] = last;
            }
        }

        private int CountLitPixels(bool[,] screen)
        {
            var lit = 0;

            for (var x = 0; x < screen.GetLength(0); x++)
            {
                for (var y = 0; y < screen.GetLength(1); y++)
                {
                    if (screen[x, y])
                    {
                        lit++;
                    }
                }
            }

            return lit;
        }

        private string GetMessage(bool[,] screen, int letterWidth)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < screen.GetLength(1); y++)
            {
                for (var x = 0; x < screen.GetLength(0); x++)
                {
                    sb.Append(screen[x, y] ? '#' : ' ');

                    if ((x + 1) % letterWidth == 0)
                    {
                        for (var i = 0; i < letterWidth; i++)
                        {
                            sb.Append(' ');
                        }
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}