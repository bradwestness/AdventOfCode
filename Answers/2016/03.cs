using System;
using System.Collections.Generic;
using advent.Input;

namespace advent.Answers._2016
{
    public class _03 : IPuzzleAnswer
    {
        private readonly IPuzzleInput _input;

        public _03(IPuzzleInput input) => _input = input;

        public string Part1()
        {
            var triangles = ParseHorizontal(_input);
            var possible = CountPossible(triangles);
            return $"Total possible triangles: {possible}.";
        }

        public string Part2()
        {
            var triangles = ParseVertical(_input);
            var possible = CountPossible(triangles);
            return $"Total possible triangles: {possible}.";
        }

        private int CountPossible(IEnumerable<Triangle> triangles)
        {
            int possible = 0;

            foreach (var triangle in triangles)
            {
                if (IsPossible(triangle))
                {
                    possible++;
                }
            }

            return possible;
        }

        private bool IsPossible(Triangle triangle) =>
            triangle.A + triangle.B > triangle.C &&
            triangle.B + triangle.C > triangle.A &&
            triangle.C + triangle.A > triangle.B;

        private IEnumerable<Triangle> ParseHorizontal(IPuzzleInput input)
        {
            List<Triangle> list = new();

            foreach (var line in input.ReadLines())
            {
                var (strA, strB, strC, _) = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                list.Add(new(int.Parse(strA), int.Parse(strB), int.Parse(strC)));
            }

            return list;
        }

        private IEnumerable<Triangle> ParseVertical(IPuzzleInput input)
        {
            List<Triangle> list = new();
            var values = new int[3, 3];
            var i = 0;

            foreach (var line in input.ReadLines())
            {
                var mod = i % 3;

                if (i > 0 && mod == 0)
                {
                    AddVerticalTriangles(list, values);
                    values.Initialize();
                }

                var (strA, strB, strC, _) = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                values[mod, 0] = int.Parse(strA);
                values[mod, 1] = int.Parse(strB);
                values[mod, 2] = int.Parse(strC);

                i++;
            }

            AddVerticalTriangles(list, values);

            return list;
        }

        private void AddVerticalTriangles(IList<Triangle> list, int[,] values)
        {
            list.Add(new(values[0, 0], values[1, 0], values[2, 0]));
            list.Add(new(values[0, 1], values[1, 1], values[2, 1]));
            list.Add(new(values[0, 2], values[1, 2], values[2, 2]));
        }

        private record Triangle(int A, int B, int C);
    }
}