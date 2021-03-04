using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using advent.Answers;

namespace advent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RootCommand command = new();
            command.Add(new Option<int>(new[] { "--year", "-y" }));
            command.Add(new Option<int>(new[] { "--day", "-d" }));
            command.Handler = CommandHandler.Create<int, int>((year, day) =>
            {
                var runner = new PuzzleAnswerRunner(Console.WriteLine);
                runner.RunPuzzleAnswer(year, day);
            });
            command.Invoke(args);
        }
    }
}
