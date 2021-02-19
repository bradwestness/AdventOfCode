namespace advent.Answers._2015
{
    public class _20 : IAnswer
    {
        private readonly uint _input;

        public _20(Input input) => _input = uint.Parse(input.ReadToEnd());

        public string Part1()
        {
            var presents = DeliverPresents(_input, modifier: 10);
            int house;

            for (house = 1; house < presents.Length; house++)
            {
                if (presents[house] > _input)
                {
                    break;
                }
            }

            return $"First house with over {_input} presents: {house}.";
        }

        public string Part2()
        {
            var presents = DeliverPresents(_input, modifier: 11, stopAfterHouses: 50);
            int house;

            for (house = 1; house < presents.Length; house++)
            {
                if (presents[house] > _input)
                {
                    break;
                }
            }

            return $"First house with over {_input} presents: {house}.";
        }

        private uint[] DeliverPresents(uint max, uint modifier, uint? stopAfterHouses = null)
        {
            var presents = new uint[max / modifier + 1];

            for (uint elf = 1; elf < presents.Length; elf++)
            {
                uint housesDeliveredTo = 0;

                for (uint house = elf; house < presents.Length; house += elf)
                {
                    presents[house] += elf * modifier;
                    housesDeliveredTo++;

                    if (stopAfterHouses.HasValue && housesDeliveredTo >= stopAfterHouses)
                    {
                        break;
                    }
                }
            }

            return presents;
        }
    }
}