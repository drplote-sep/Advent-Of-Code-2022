using System.Diagnostics;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public abstract class DayRunner
    {
        private readonly DayData _data;

        protected DayRunner(DayData data)
        {
            _data = data;
        }

        public void Go(bool useTestData = false)
        {
            OutputWriter.WriteDayHeader(_data.DayNumber);
            var stopwatch = Stopwatch.StartNew();
            SolveDay(useTestData ? _data.GetTestData() : _data.GetRealData());
            stopwatch.Stop();
            OutputWriter.WriteTimeResult(stopwatch.ElapsedMilliseconds);
        }

        protected abstract void SolveDay(string[] data);
    }
}