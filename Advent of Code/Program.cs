using System.Collections.Generic;
using Advent_of_Code.DataSources;
using Advent_of_Code.DayRunners;

namespace Advent_of_Code
{
    internal class Day1Program
    {
        private static void Main(string[] args)
        {
            OutputWriter.WriteHeader("Advent of Code 2022");

            foreach (var dayRunner in GetDayRunners())
            {
                dayRunner.Go(false);
            }
        }

        private static List<DayRunner> GetDayRunners()
        {
            return new List<DayRunner>
            {
                new Day1Runner(new DayData(1)),
            };
        }
    }
}