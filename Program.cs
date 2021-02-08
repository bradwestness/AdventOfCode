using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace advent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var command = new RootCommand();
            command.Add(new Option<int>(new[] { "--year", "-y" }));
            command.Add(new Option<int>(new[] { "--day", "-d" }));
            command.Handler = CommandHandler.Create<int, int>(RunAnswer);
            command.Invoke(args);
        }

        private static void RunAnswer(int year, int day)
        {
            if (year < 100)
            {
                year += 2000;
            }

            var className = $"advent.Answers._{year:0000}._{day:00}";
            var @interface = typeof(Answers.IAnswer);
            var implementations = @interface.Assembly.GetTypes().Where(t => @interface.IsAssignableFrom(t));
            var implementation = implementations.FirstOrDefault(i => i.FullName.Equals(className));
            var inputCtor = implementation?.GetConstructor(new Type[] { typeof(string) });
            var parameterlessCtor = implementation?.GetConstructor(new Type[] { });
            var instance = (inputCtor is object)
                ? inputCtor?.Invoke(new object[] { LoadInput(year, day) }) as Answers.IAnswer
                : parameterlessCtor?.Invoke(new object[] { }) as Answers.IAnswer;

            if (implementation is null)
            {
                throw new Exception($"No answer found with year {year} and day {day}!");
            }

            if (instance is null)
            {
                throw new Exception($"Answer for year {year} and day {day} does not implement IAnswer!");
            }

            Console.WriteLine($"\n\tRunning answer for year {year}, day {day}...");

            RunPart(instance.Part1);
            RunPart(instance.Part2);

            Console.WriteLine();
        }

        private static void RunPart(Func<string> part)
        {
            var methodName = (ReadOnlySpan<char>)part.Method.Name;
            var partNumber = methodName[methodName.Length - 1];

            Console.WriteLine($"\n\tRunning part {partNumber}...");

            var sw = Stopwatch.StartNew();
            var result = part();
            sw.Stop();

            Console.WriteLine($"\t\t{result}");
            Console.WriteLine($"\t\tElapsed milliseconds: {sw.ElapsedMilliseconds}.");
        }

        private static string LoadInput(int year, int day)
        {
            var assembly = typeof(Program).Assembly;
            var names = assembly.GetManifestResourceNames();
            var resourceName = $"advent.Inputs._{year:0000}.{day:00}.txt";

            using (var resource = (Stream)assembly.GetManifestResourceStream(resourceName))
            using (var sr = new StreamReader(resource))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
