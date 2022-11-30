using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Advent_of_Code.DataSources;
using Advent_of_Code.DayRunners;

namespace Advent_of_Code
{
    internal class Day1Program
    {
        private static void Main(string[] args)
        {
            OutputWriter.WriteHeader("Advent of Code 2022");

            foreach (var dayRunner in GetDayRunners(1))
            {
                dayRunner.Go(true);
            }
        }

        private static List<DayRunner> GetDayRunners(int maxDayNumber, int minDayNumber = 1)
        {
            var dayRunners = new List<DayRunner>();
            for (int i = minDayNumber; i <= maxDayNumber; i++)
            {
                dayRunners.Add(CreateDayRunner(i));
            }

            return dayRunners;
        }

        public static DayRunner CreateDayRunner(int dayNumber)
        {
            var runnerType = Assembly.GetExecutingAssembly().GetTypes()
                .SingleOrDefault(t => t.Name == $"Day{dayNumber}Runner");

            if (runnerType == null)
            {
                throw new ArgumentException($"No DayRunner found for day {dayNumber}");
            }

            return Activator.CreateInstance(runnerType, new DayData(dayNumber)) as DayRunner;
        }
    }
}