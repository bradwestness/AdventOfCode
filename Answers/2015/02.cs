using System.Linq;

namespace advent.Answers._2015
{
    public class _02 : IAnswer
    {
        private readonly string _input;

        public _02(string input)
        {
            _input = input;
        }

        public string Part1()
        {
            long total = 0;

            foreach (var line in _input.ToLines())
            {
                var (lToken, wToken, hToken, _) = line.Split("x");

                int.TryParse(lToken, out var l);
                int.TryParse(wToken, out var w);
                int.TryParse(hToken, out var h);

                var side1 = l * w;
                var side2 = w * h;
                var side3 = h * l;
                var area = (2 * side1) + (2 * side2) + (2 * side3);
                var slack = new[] { side1, side2, side3 }.Min();

                total += (area + slack);
            }

            return $"Total wrapping paper to buy: {total} feet.";
        }

        public string Part2()
        {
            long total = 0;

            foreach (var line in _input.ToLines())
            {
                var (lToken, wToken, hToken, _) = line.Split("x");

                int.TryParse(lToken, out var l);
                int.TryParse(wToken, out var w);
                int.TryParse(hToken, out var h);

                var (side1, side2, _) = new[] { l, w, h }.OrderBy(x => x);
                var perimiter = (side1 * 2) + (side2 * 2);
                var bow = l * w * h;
                var ribbon = perimiter + bow;

                total += ribbon;
            }

            return $"Total ribbon to buy: {total} feet.";
        }
    }
}
