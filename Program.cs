using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace advent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var command = new RootCommand();
            command.Add(new Option<int>(new[] { "--year", "-y" }));
            command.Add(new Option<int>(new[] { "--day", "-d" }));
            command.Handler = CommandHandler.Create<int, int>(Run);
            command.Invoke(args);
        }

        private static void Run(int year, int day)
        {
            if (year < 100)
            {
                year += 2000;
            }
            
            var className = $"advent._{year:0000}._{day:00}";
            var @interface = typeof(IAnswer);
            var implementations = @interface.Assembly.GetTypes().Where(t => @interface.IsAssignableFrom(t));
            var implementation = implementations.FirstOrDefault(i => i.FullName.Equals(className));
            var ctor = implementation?.GetConstructor(new Type[] { });
            var instance = ctor?.Invoke(new object[] { }) as IAnswer;

            if (implementation is null)
            {
                throw new Exception($"\tNo answer found with year {year} and day {day}!");
            }

            if (ctor is null)
            {
                throw new Exception($"\tAnswer for year {year} and day {day} has no parameterless constructor!");
            }

            if (instance is null)
            {
                throw new Exception($"\tAnswer for year {year} and day {day} does not implement IAnswer!");
            }

            instance.Run();
        }
    }
}
