using System;
using System.Collections.Generic;

namespace advent.Answers._2015
{
    public class _07 : IAnswer
    {
        private readonly IDictionary<string, ushort?> _wires;
        private readonly IDictionary<string, Instruction> _instructions;

        public _07(string input)
        {
            _wires = new Dictionary<string, ushort?>();
            _instructions = ParseInstructions(input);
        }

        public string Part1()
        {
            var result = GetWireSignal("a");
            return $"Signal on wire a: {result}.";
        }

        public string Part2()
        {
            var a = GetWireSignal("a");
            _wires.Clear();
            _wires.Add("b", a);

            var result = GetWireSignal("a");
            return $"Signal on wire a: {result}.";
        }

        private ushort? GetWireSignal(string wire)
        {
            if (_wires.ContainsKey(wire))
            {
                return _wires[wire];
            }

            var instruction = _instructions[wire];
            var leftSignal = GetOperandSignal(instruction.LeftOperand);
            var rightSignal = GetOperandSignal(instruction.RightOperand);
            var result = Gate(leftSignal, instruction.Operator, rightSignal);
            _wires.Add(wire, result);

            return result;
        }

        private ushort? GetOperandSignal(string operand)
        {
            if (string.IsNullOrWhiteSpace(operand))
            {
                return null;
            }

            var isOperandInt = ushort.TryParse(operand, out var signal);
            if (isOperandInt)
            {
                return signal;
            }

            return GetWireSignal(operand);
        }

        private enum Operator
        {
            And,
            LShift,
            Not,
            Or,
            RShift
        }

        private static Operator? ParseOperator(string line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                foreach (Operator @operator in Enum.GetValues<Operator>())
                {
                    var name = Enum.GetName<Operator>(@operator);

                    if (line.Contains(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return @operator;
                    }
                }
            }

            return null;
        }

        private record Instruction(
            string LeftOperand,
            Operator? Operator,
            string RightOperand
        );

        private static IDictionary<string, Instruction> ParseInstructions(string input)
        {
            var result = new Dictionary<string, Instruction>();

            foreach (var line in input.ToLines())
            {
                var (inputs, wire, _) = line.Split(" -> ");
                var tokens = inputs.Split(" ");
                string leftOperand = null;
                string operatorStr = null;
                string rightOperand = null;

                switch (tokens.Length)
                {
                    case 1:
                        (leftOperand, _) = tokens;
                        break;

                    case 2:
                        (operatorStr, rightOperand, _) = tokens;
                        break;

                    case 3:
                        (leftOperand, operatorStr, rightOperand, _) = tokens;
                        break;
                }

                Operator? @operator = null;
                if (!string.IsNullOrWhiteSpace(operatorStr) && Enum.TryParse<Operator>(operatorStr, true, out var operatorEnum))
                {
                    @operator = operatorEnum;
                }

                result.Add(wire, new Instruction(leftOperand, @operator, rightOperand));
            }

            return result;
        }

        private static ushort? Gate(ushort? leftSignal, Operator? @operator, ushort? rightSignal) =>
            @operator switch
            {
                Operator.And => (leftSignal.HasValue && rightSignal.HasValue)
                    ? (ushort)(leftSignal.Value & rightSignal.Value)
                    : null,
                Operator.LShift => (leftSignal.HasValue && rightSignal.HasValue)
                    ? (ushort)(leftSignal.Value << rightSignal.Value)
                    : null,
                Operator.Not => rightSignal.HasValue
                    ? (ushort)(~rightSignal.Value)
                    : null,
                Operator.Or => (leftSignal.HasValue && rightSignal.HasValue)
                    ? (ushort)(leftSignal.Value | rightSignal.Value)
                    : null,
                Operator.RShift => (leftSignal.HasValue && rightSignal.HasValue)
                    ? (ushort)(leftSignal.Value >> rightSignal.Value)
                    : null,
                _ => leftSignal
            };
    }
}