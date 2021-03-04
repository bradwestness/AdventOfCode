using System.Collections.Generic;

namespace advent.Input
{
    public interface IPuzzleInput
    {
        int Year { get; }
        int Day { get; }
        string ReadToEnd();
        IEnumerable<string> ReadLines();
    }
}