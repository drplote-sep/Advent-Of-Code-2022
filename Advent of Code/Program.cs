using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Advent_of_Code.DataSources;
using Advent_of_Code.DayRunners;

namespace Advent_of_Code
{
    internal static class Program
    {
        private static readonly bool ShouldUseTestData = false;
        private static readonly int? SingleDayRun = 11;
        
        private static void Main(string[] args)
        {
            OutputWriter.WriteHeader("Advent of Code 2022");

            if (SingleDayRun.HasValue)
            {
                RunDay(SingleDayRun.Value);
            }
            else
            {
                foreach (var dayRunner in GetDayRunners(12))
                {
                    dayRunner.Go(ShouldUseTestData);
                }    
            }
        }

        private static void RunDay(int dayNumber)
        {
            CreateDayRunner(dayNumber).Go(ShouldUseTestData);
        }

        private static List<DayRunner> GetDayRunners(int maxDayNumber, int minDayNumber = 1)
        {
            var dayRunners = new List<DayRunner>();
            for (int i = minDayNumber; i <= maxDayNumber; i++)
            {
                var dayRunner = CreateDayRunner(i);
                if (dayRunner != null)
                {
                    dayRunners.Add(CreateDayRunner(i));    
                }
                
            }

            return dayRunners;
        }

        private static DayRunner CreateDayRunner(int dayNumber)
        {
            var runnerType = Assembly.GetExecutingAssembly().GetTypes()
                .SingleOrDefault(t => t.Name == $"Day{dayNumber}Runner");

            var dayData = new DayData(dayNumber);
            if (runnerType == null)
            {
                return new IncompleteDayRunner(dayData);
            }

            return Activator.CreateInstance(runnerType, dayData) as DayRunner;
        }
    }
}