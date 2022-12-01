using System;
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
            var elves = GetElves(data);

            var orderedElves = elves.OrderByDescending(e => e.TotalCalories);
            OutputWriter.WriteResult(1, $"The highest total calories for an elf is {orderedElves.First().TotalCalories}");

            var top3 = orderedElves.Take(3);
            OutputWriter.WriteResult(2, $"The total calories for the top 3 elves is {top3.Select(e => e.TotalCalories).Sum()}");
        }

        private List<Elf> GetElves(string[] data)
        {
            var elves = new List<Elf>();
            var currentElf = new Elf();
            elves.Add(currentElf);
            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentElf = new Elf();
                    elves.Add(currentElf);
                }
                else
                {
                    currentElf.Calories.Add(Convert.ToInt32(line));
                }
            }
            
            return elves;
        }
    }

    public class Elf
    {
        public List<int> Calories { get; private set; } = new List<int>();
        
        public int TotalCalories => Calories.Sum();

        public Elf()
        {
        }
    }
}