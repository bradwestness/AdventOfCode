using System;
using System.Diagnostics;
using System.Linq;
using advent.Input;

namespace advent.Answers
{
    public class PuzzleAnswerRunner
    {
        private readonly Action<string> _outputWriter;

        public PuzzleAnswerRunner(Action<string> outputWriter) =>
            _outputWriter = outputWriter;

        public void RunPuzzleAnswer(int year, int day)
        {
            if (year < 100)
            {
                year += 2000;
            }

            var className = $"advent.Answers._{year:0000}._{day:00}";
            var @interface = typeof(Answers.IPuzzleAnswer);
            var implementations = @interface.Assembly.GetTypes().Where(t => @interface.IsAssignableFrom(t));
            var implementation = implementations.FirstOrDefault(i => i.FullName.Equals(className));
            var streamCtor = implementation?.GetConstructor(new Type[] { typeof(IPuzzleInput) });
            var parameterlessCtor = implementation?.GetConstructor(new Type[] { });
            var instance = (streamCtor is object)
                    ? streamCtor?.Invoke(new object[] { new ManifestResourcePuzzleInput(year, day) }) as Answers.IPuzzleAnswer
                    : parameterlessCtor?.Invoke(new object[] { }) as Answers.IPuzzleAnswer;

            if (implementation is null)
            {
                throw new Exception($"No puzzle answer found with year {year} and day {day}!");
            }

            if (instance is null)
            {
                throw new Exception($"Puzzle answer for year {year} and day {day} does not implement IPuzzleAnswer!");
            }

            WriteOutput($"\n\tRunning puzzle answer for year {year}, day {day}...");

            RunPart(instance.Part1);
            RunPart(instance.Part2);

            WriteOutput(string.Empty);
        }

        private void RunPart(Func<string> part)
        {
            var methodName = (ReadOnlySpan<char>)part.Method.Name;
            var partNumber = methodName[methodName.Length - 1];

            WriteOutput($"\n\tRunning part {partNumber}...");

            var sw = Stopwatch.StartNew();
            var result = part();
            sw.Stop();

            WriteOutput($"\t\t{result}");
            WriteOutput($"\t\tElapsed milliseconds: {sw.ElapsedMilliseconds}.");
        }

        private void WriteOutput(string value) => _outputWriter(value);
    }
}