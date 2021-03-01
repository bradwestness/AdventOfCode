using System;
using System.Collections.Generic;

namespace advent.Answers._2016
{
    public class _01 : IAnswer
    {
        private readonly string[] _instructions;

        public _01(Input input) =>
            _instructions = input.ReadToEnd().Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        public string Part1()
        {
            Intersection start = new(0, 0);
            var end = FollowInstructions(_instructions, start);
            var distance = GetBlocksAway(start, end);
            return $"Number of blocks from the starting position: {distance}";
        }

        public string Part2()
        {
            Intersection start = new(0, 0);
            var end = FollowInstructions(_instructions, start, stopAtFirstRevisit: true);
            var distance = GetBlocksAway(start, end);
            return $"Number of blocks from the starting position: {distance}";
        }

        private Intersection FollowInstructions(IEnumerable<string> instructions, Intersection start, bool stopAtFirstRevisit = false)
        {
            var position = start with { };
            var facing = Direction.N;
            var history = stopAtFirstRevisit
                ? new List<Intersection> { start }
                : null;

            foreach (var instruction in instructions)
            {
                var (turn, blocksStr) = instruction.ToCharArray();
                var blocksToWalk = int.Parse(string.Join("", blocksStr));
                facing = Turn(facing, turn);

                var blocksWalked = 0;
                while (blocksWalked < blocksToWalk)
                {
                    position = WalkOneBlock(position, facing);

                    if (stopAtFirstRevisit)
                    {
                        if (history.Contains(position))
                        {
                            return position;
                        }

                        history.Add(position);
                    }

                    blocksWalked++;
                }
            }

            return position;
        }

        private int GetBlocksAway(Intersection start, Intersection end) =>
            Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);

        private Direction Turn(Direction facing, char turn) => facing switch
        {
            Direction.N when turn == 'R' => Direction.E,
            Direction.N when turn == 'L' => Direction.W,

            Direction.E when turn == 'R' => Direction.S,
            Direction.E when turn == 'L' => Direction.N,

            Direction.S when turn == 'R' => Direction.W,
            Direction.S when turn == 'L' => Direction.E,

            Direction.W when turn == 'R' => Direction.N,
            Direction.W when turn == 'L' => Direction.S,

            _ => facing
        };

        private Intersection WalkOneBlock(Intersection point, Direction facing) => facing switch
        {
            Direction.N => point with { X = point.X + 1 },
            Direction.E => point with { Y = point.Y + 1 },
            Direction.S => point with { X = point.X - 1 },
            Direction.W => point with { Y = point.Y - 1 },
            _ => point
        };

        private enum Direction
        {
            N,
            E,
            S,
            W
        };

        private record Intersection(int X, int Y);
    }
}