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
            using (Stream stream = typeof(Input).Assembly.GetManifestResourceStream(_resourceName))
            using (StreamReader sr = new(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public IEnumerable<string> ReadLines()
        {
            using (Stream stream = typeof(Input).Assembly.GetManifestResourceStream(_resourceName))
            using (StreamReader sr = new(stream))
            {
                while (sr.Peek() != -1)
                {
                    yield return sr.ReadLine();
                }
            }
        }
    }
}