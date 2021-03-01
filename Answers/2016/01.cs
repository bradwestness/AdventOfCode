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
            var end = FollowInstructions(_instructions, start, Direction.N);
            var blocks = GetBlocksAway(start, end);
            return $"Number of blocks from the starting position: {blocks}";
        }

        public string Part2()
        {
            Intersection start = new(0, 0);
            var end = FollowInstructions(_instructions, start, Direction.N, stopAtFirstRevisit: true);
            var blocks = GetBlocksAway(start, end);
            return $"Number of blocks from the starting position: {blocks}";
        }

        private Intersection FollowInstructions(IEnumerable<string> instructions, Intersection start, Direction facing, bool stopAtFirstRevisit = false)
        {
            var position = start with { };
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

        private Intersection WalkOneBlock(Intersection position, Direction facing) => facing switch
        {
            Direction.N => position with { X = position.X + 1 },
            Direction.E => position with { Y = position.Y + 1 },
            Direction.S => position with { X = position.X - 1 },
            Direction.W => position with { Y = position.Y - 1 },
            _ => position
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