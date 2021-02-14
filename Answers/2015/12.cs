using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace advent.Answers._2015
{
    public class _12 : IAnswer
    {
        private readonly string _input;

        public _12(Input input)
        {
            _input = input.ReadToEnd();
        }

        public string Part1()
        {
            var total = SumAllNumbers(_input);
            return $"Total of all numbers: {total}.";
        }

        public string Part2()
        {
            var total = SumAllExceptRed(_input);
            return $"Total of all not-red numbers: {total}";
        }

        private int SumAllNumbers(ReadOnlySpan<char> json)
        {
            var total = 0;
            var i = 0;
            var j = 0;

            for (i = 0; i < json.Length; i++)
            {
                j = i;

                while (j < json.Length && (json[j] == '-' || char.IsNumber(json[j])))
                {
                    j++;
                }

                if (j > i && int.TryParse(json.Slice(i, j - i), out var number))
                {
                    total += number;
                }

                i = j;
            }

            return total;
        }

        private int SumAllExceptRed(string json)
        {
            int total = 0;
            var root = JsonSerializer.Deserialize<JsonElement>(json);

            SumAllExceptColor(ref total, root, Color.Red);

            return total;
        }

        private void SumAllExceptColor(ref int total, JsonElement element, Color color)
        {
            if (IsColor(element, color))
            {
                return;
            }

            switch (element.ValueKind)
            {
                case JsonValueKind.Array:
                    foreach (var arrayElement in element.EnumerateArray())
                    {
                        SumAllExceptColor(ref total, arrayElement, color);
                    }
                    break;

                case JsonValueKind.Number:
                    element.TryGetInt32(out var intVal);
                    total += intVal;
                    break;

                case JsonValueKind.Object:
                    foreach (var objectProperty in element.EnumerateObject())
                    {
                        SumAllExceptColor(ref total, objectProperty.Value, color);
                    }
                    break;
            }
        }

        private enum Color
        {
            Red
        }

        private bool IsColor(JsonElement element, Color color) => element.ValueKind switch
        {
            JsonValueKind.String => element.GetString().Equals(Enum.GetName(color), StringComparison.OrdinalIgnoreCase),
            JsonValueKind.Object => element.EnumerateObject().Any(p => p.Value.ValueKind == JsonValueKind.String && p.Value.GetString().Equals(Enum.GetName(color), StringComparison.OrdinalIgnoreCase)),
            _ => false
        };
    }
}