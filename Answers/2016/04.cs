using System;
using System.Collections.Generic;
using System.Linq;

namespace advent.Answers._2016
{
    public class _04 : IAnswer
    {
        private readonly Input _input;

        public _04(Input input) => _input = input;

        public string Part1()
        {
            int sum = 0;
            foreach (var line in _input.ReadLines())
            {
                if (TryGetSectorId(line, out var sectorId))
                {
                    sum += sectorId;
                }
            }
            return $"Sum of real room sector IDs: {sum}.";
        }

        public string Part2()
        {
            var roomName = "northpole-object-storage";
            var roomSectorId = 0;
            foreach (var line in _input.ReadLines())
            {
                if (TryGetSectorId(line, out var sectorId))
                {
                    var decrypted = DecryptRoomName(line, sectorId);
                    if (decrypted.Contains(roomName))
                    {
                        roomSectorId = sectorId;
                        break;
                    }
                }
            }
            return $"Sector ID of room where North Pole objects are stored: {roomSectorId}.";
        }

        private bool TryGetSectorId(ReadOnlySpan<char> line, out int sectorId)
        {
            Dictionary<char, int> chars = new();
            var i = 0;

            while (char.IsLetter(line[i]) || line[i] == '-')
            {
                if (char.IsLetter(line[i]))
                {
                    if (!chars.ContainsKey(line[i]))
                    {
                        chars.Add(line[i], 0);
                    }

                    chars[line[i]]++;
                }

                i++;
            }

            var (sectorIdStr, checksum, _) = new string(line.Slice(i))
                .Split(new[] { '-', '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var top5 = string.Concat(chars
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key)
                .Select(x => x.Key)
                .Take(5));

            if (top5.Equals(checksum))
            {
                return int.TryParse(sectorIdStr, out sectorId);
            }

            sectorId = -1;
            return false;
        }

        private string DecryptRoomName(ReadOnlySpan<char> line, int sectorId)
        {
            var chars = new char[line.Length];

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (char.IsLetter(c))
                {
                    for (var j = 0; j < sectorId; j++)
                    {
                        c = c switch
                        {
                            'z' => 'a',
                            _ => (char)((int)c + 1)
                        };
                    }
                }

                chars[i] = c;
            }

            return string.Concat(chars);
        }
    }
}