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
            OutputWriter.WriteResult(1, $"MY BODY");
            OutputWriter.WriteResult(2, $"IS READY");
        }
    }
}