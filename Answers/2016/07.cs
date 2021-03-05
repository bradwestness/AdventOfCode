using System;
using System.Collections.Generic;
using advent.Input;

namespace advent.Answers._2016
{
    public class _07 : IPuzzleAnswer
    {
        private readonly IPuzzleInput _input;

        public _07(IPuzzleInput input) => _input = input;

        public string Part1()
        {
            var count = 0;
            foreach (var line in _input.ReadLines())
            {
                if (SupportsTransportLayerSnooping(line))
                {
                    count++;
                }
            }
            return $"Number of IPs supporting TLS: {count}.";
        }

        public string Part2()
        {
            var count = 0;
            foreach (var line in _input.ReadLines())
            {
                if (SupportsSuperSecretListening(line))
                {
                    count++;
                }
            }
            return $"Number of IPs supporting SSL: {count}.";
        }

        private bool SupportsTransportLayerSnooping(string line)
        {
            var valid = 0;

            foreach (var position in EnumerateAutonomousBridgeBypassAnnotationStartPositions(line))
            {
                if (IsWithinHypernetSequence(line, position))
                {
                    return false;
                }

                valid++;
            }

            return valid > 0;
        }

        private IEnumerable<int> EnumerateAutonomousBridgeBypassAnnotationStartPositions(string line)
        {
            for (var i = 0; i < line.Length - 3; i++)
            {
                if (line[i] == line[i + 3] &&
                    line[i + 1] == line[i + 2] &&
                    line[i] != line[i + 1])
                {
                    yield return i;
                }
            }
        }

        private bool SupportsSuperSecretListening(string line)
        {
            foreach (var position in EnumerateAreaBroadcastAccessorStartPositions(line))
            {
                var areaBroadcastAccessor = line.Substring(position, 3);
                var byteAllocationBlock = new string(new char[] { areaBroadcastAccessor[1], areaBroadcastAccessor[0], areaBroadcastAccessor[1] });

                if (HasByteAllocationBlock(line, byteAllocationBlock))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<int> EnumerateAreaBroadcastAccessorStartPositions(string line)
        {
            for (var i = 0; i < line.Length - 2; i++)
            {
                if (line[i] == line[i + 2] &&
                    line[i] != line[i + 1] &&
                    !IsWithinHypernetSequence(line, i))
                {
                    yield return i;
                }
            }
        }

        private bool HasByteAllocationBlock(string line, string byteAllocationBlock)
        {
            var startPosition = line.IndexOf(byteAllocationBlock);

            while (startPosition != -1)
            {
                if (IsWithinHypernetSequence(line, startPosition))
                {
                    return true;
                }

                line = line.Substring(startPosition + byteAllocationBlock.Length - 1);
                startPosition = line.IndexOf(byteAllocationBlock);
            }

            return false;
        }

        private bool IsWithinHypernetSequence(ReadOnlySpan<char> line, int startPosition)
        {
            var before = line.Slice(0, startPosition);
            var after = line.Slice(startPosition);
            var open = before.LastIndexOf('[');
            if (open == -1)
            {
                return false;
            }

            var close = after.IndexOf(']');
            if (close == -1)
            {
                return false;
            }

            open = after.IndexOf('[');

            if (open != -1 && open < close)
            {
                return false;
            }

            return true;
        }
    }
}