using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day1Runner : DayRunner
    {
        public Day1Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            CountIncreases(data);
            CountSlidingIncrease(data);
        }

        private static void CountSlidingIncrease(string[] lines)
        {
            var windows = new Dictionary<int, List<int>>();
            var currentWindow = 0;

            foreach (var line in lines)
            {
                var lineValue = int.Parse(line);
                windows[currentWindow] = new List<int> {lineValue};

                if (currentWindow > 0) windows[currentWindow - 1].Add(lineValue);

                if (currentWindow > 1) windows[currentWindow - 2].Add(lineValue);

                currentWindow++;
            }

            var sums = windows.Values.Select(v => v.Sum());

            OutputWriter.WriteResult(2, $"Total # of Window Increases is {CountIncreases(sums)}");
        }

        private static int CountIncreases(IEnumerable<int> nums)
        {
            int? current = null;
            var increaseCount = 0;

            foreach (var num in nums)
            {
                var previous = current;
                current = num;

                if (previous != null && current > previous) increaseCount++;
            }

            return increaseCount;
        }

        private static void CountIncreases(string[] lines)
        {
            var nums = lines.Select(int.Parse);
            OutputWriter.WriteResult(1, $"Day 1.1: Total # of Increases is {CountIncreases(nums)}");
        }
    }
}