using System.Collections.Generic;
using System.IO;

namespace advent.Answers
{
    public class Input
    {
        private readonly string _resourceName;

        public Input(int year, int day)
        {
            _resourceName = $"advent.Inputs._{year:0000}.{day:00}.txt";
        }

        public string ReadToEnd()
        {
            using (var stream = typeof(Input).Assembly.GetManifestResourceStream(_resourceName))
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public IEnumerable<string> ReadLines()
        {
            using (var stream = typeof(Input).Assembly.GetManifestResourceStream(_resourceName))
            using (var sr = new StreamReader(stream))
            {
                while (sr.Peek() != -1)
                {
                    yield return sr.ReadLine();
                }
            }
        }
    }
}