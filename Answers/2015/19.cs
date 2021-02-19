using System;
using System.Collections.Generic;

namespace advent.Answers._2015
{
    public class _19 : IAnswer
    {
        private readonly IList<KeyValuePair<string, string>> _replacements;
        private readonly string _medicine;

        public _19(Input input)
        {
            var (replacements, medicine) = ParseInput(input);
            _replacements = replacements;
            _medicine = medicine;
        }

        public string Part1()
        {
            var molecules = GetDistinctMolecules(_replacements, _medicine);
            return $"Distinct molecules that may be created from replacements: {molecules.Count}.";
        }

        public string Part2()
        {
            var steps = GetStepsToProduceMedicine(_replacements, "e", _medicine);
            return $"Steps required to produce molecule: {steps}.";
        }

        private IList<string> GetDistinctMolecules(IList<KeyValuePair<string, string>> replacements, string medicine)
        {
            List<string> molecules = new();

            foreach (var replacement in replacements)
            {
                var (key, value) = replacement;
                var i = 0;
                var keyIndex = medicine.IndexOf(key, i);

                while (i < medicine.Length + key.Length && keyIndex != -1)
                {
                    var molecule = medicine.Remove(keyIndex, key.Length).Insert(keyIndex, value);

                    if (!molecules.Contains(molecule))
                    {
                        molecules.Add(molecule);
                    }

                    i = keyIndex + key.Length;
                    keyIndex = medicine.IndexOf(key, i);
                }
            }

            return molecules;
        }

        private int GetStepsToProduceMedicine(IList<KeyValuePair<string, string>> replacements, string start, string medicine)
        {
            // beginning with the desired output (the medicine),
            // work backward making replacements at random
            // until we arrive at the specified starting molecule

            var molecule = medicine;
            var random = new Random(Environment.TickCount);
            var steps = 0;

            while (molecule != start)
            {
                var r = random.Next(replacements.Count);
                var (key, value) = replacements[r];
                var valueIndex = molecule.IndexOf(value);

                if (valueIndex != -1)
                {
                    molecule = molecule.Remove(valueIndex, value.Length).Insert(valueIndex, key);
                    steps++;
                }
            }

            return steps;
        }

        private (IList<KeyValuePair<string, string>> Replacements, string Medicine) ParseInput(Input input)
        {
            List<KeyValuePair<string, string>> replacements = new();
            var medicine = string.Empty;

            foreach (var line in input.ReadLines())
            {
                var (key, value, _) = line.Split(" => ");
                key = key?.Trim() ?? string.Empty;
                value = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(key))
                {
                    // this is a blank line
                    continue;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    // this is the medicine molecule
                    medicine = key;
                    continue;
                }

                // this is a replacement
                replacements.Add(new KeyValuePair<string, string>(key, value));
            }

            return (replacements, medicine);
        }
    }
}