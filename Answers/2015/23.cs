using System;

namespace advent.Answers._2015
{
    public class _23 : IAnswer
    {
        private readonly string[] _lines;

        public _23(Input input) => _lines = input.ReadToEnd().Split('\n');

        public string Part1()
        {
            uint a = 0;
            uint b = 0;
            ProcessInstructions(ref a, ref b);

            return $"The value of register B is: {b}.";
        }

        public string Part2()
        {
            uint a = 1;
            uint b = 0;
            ProcessInstructions(ref a, ref b);

            return $"The value of register B is: {b}.";
        }

        private void ProcessInstructions(ref uint a, ref uint b)
        {
            for (var i = 0; i < _lines.Length; i++)
            {
                var instruction = _lines[i].Substring(0, 3);
                var register = _lines[i].Substring(4, 1);

                switch (instruction)
                {
                    case "hlf":
                        ProcessInstruction(x => x / 2, register, ref a, ref b);
                        break;

                    case "tpl":
                        ProcessInstruction(x => x * 3, register, ref a, ref b);
                        break;

                    case "inc":
                        ProcessInstruction(x => x + 1, register, ref a, ref b);
                        break;

                    case "jmp":
                        if (int.TryParse(_lines[i].Substring(4), out var jmp))
                        {
                            i += jmp - 1;
                        }
                        break;

                    case "jie":
                        if (IsEven(register, ref a, ref b) && int.TryParse(_lines[i].Substring(7), out var jie))
                        {
                            i += jie - 1;
                        }
                        break;

                    case "jio":
                        if (IsOne(register, ref a, ref b) && int.TryParse(_lines[i].Substring(7), out var jio))
                        {
                            i += jio - 1;
                        }
                        break;

                    default:
                        throw new NotSupportedException($@"Unsupported instruction ""{instruction}"" on line {i + 1} of input file.");
                }
            }
        }

        private void ProcessInstruction(Func<uint, uint> fn, string register, ref uint a, ref uint b)
        {
            switch (register)
            {
                case nameof(a):
                    a = fn(a);
                    break;

                case nameof(b):
                    b = fn(b);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(register));
            }
        }

        private bool IsEven(string register, ref uint a, ref uint b) => register switch
        {
            nameof(a) => a % 2 == 0,
            nameof(b) => b % 2 == 0,
            _ => throw new ArgumentOutOfRangeException(nameof(register))
        };

        private bool IsOne(string register, ref uint a, ref uint b) => register switch
        {
            nameof(a) => a == 1,
            nameof(b) => b == 1,
            _ => throw new ArgumentOutOfRangeException(nameof(register))
        };
    }
}