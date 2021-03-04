using System.Collections.Generic;
using System.IO;

namespace advent.Input
{
    public class ManifestResourcePuzzleInput : IPuzzleInput
    {
        private readonly int _year;
        private readonly int _day;

        public ManifestResourcePuzzleInput(int year, int day)
        {
            _year = year;
            _day = day;
        }

        public int Year => _year;

        public int Day => _day;

        public string ReadToEnd()
        {
            using (Stream stream = GetStream())
            using (StreamReader sr = new(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public IEnumerable<string> ReadLines()
        {
            using (Stream stream = GetStream())
            using (StreamReader sr = new(stream))
            {
                while (sr.Peek() != -1)
                {
                    yield return sr.ReadLine();
                }
            }
        }

        private Stream GetStream() =>
            typeof(ManifestResourcePuzzleInput).Assembly.GetManifestResourceStream(ResourceName);

        private string ResourceName =>
            $"advent.Input._{_year:0000}.{_day:00}.txt";
    }
}