using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class IncompleteDayRunner : DayRunner
    {
        public IncompleteDayRunner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            OutputWriter.WriteError("Didn't do this day (yet)");
        }
    }
}