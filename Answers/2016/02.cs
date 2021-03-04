using System;
using System.Text;
using advent.Input;

namespace advent.Answers._2016
{
    public class _02 : IPuzzleAnswer
    {
        private readonly IPuzzleInput _input;

        public _02(IPuzzleInput input) => _input = input;

        public string Part1()
        {
            var keypad = new char[3, 3]
            {
                { '1', '2', '3' },
                { '4', '5', '6' },
                { '7', '8', '9' }
            };
            var code = GetCode(keypad, '5');
            return $"The bathroom code is {code}.";
        }

        public string Part2()
        {
            var keypad = new char[5, 5]
            {
                { ' ', ' ', '1', ' ', ' ' },
                { ' ', '2', '3', '4', ' ' },
                { '5', '6', '7', '8', '9' },
                { ' ', 'A', 'B', 'C', ' ' },
                { ' ', ' ', 'D', ' ', ' ' }
            };
            var code = GetCode(keypad, '5');
            return $"The bathroom code is {code}.";
        }

        private string GetCode(char[,] keypad, char start)
        {
            var (x, y) = GetStartingPosition(keypad, start);
            StringBuilder sb = new();

            foreach (ReadOnlySpan<char> line in _input.ReadLines())
            {
                foreach (var instruction in line)
                {
                    (x, y) = Move(keypad, instruction, x, y);
                }

                sb.Append(keypad[x, y]);
            }

            return sb.ToString();
        }

        private (int X, int Y) GetStartingPosition(char[,] keypad, char start)
        {
            for (int x = 0; x < keypad.GetLength(0); x++)
            {
                for (int y = 0; y < keypad.GetLength(1); y++)
                {
                    if (keypad[x, y] == start)
                    {
                        return (x, y);
                    }
                }
            }

            throw new ArgumentOutOfRangeException(nameof(start));
        }

        private (int X, int Y) Move(char[,] keypad, char instruction, int x, int y)
        {
            var (newX, newY) = instruction switch
            {
                'U' => (x - 1, y),
                'D' => (x + 1, y),
                'L' => (x, y - 1),
                'R' => (x, y + 1),
                _ => (x, y)
            };

            if (newX < 0 || newX >= keypad.GetLength(0))
            {
                return (x, y);
            }

            if (newY < 0 || newY >= keypad.GetLength(1))
            {
                return (x, y);
            }

            if (keypad[newX, newY] == ' ')
            {
                return (x, y);
            }

            return (newX, newY);
        }
    }
}