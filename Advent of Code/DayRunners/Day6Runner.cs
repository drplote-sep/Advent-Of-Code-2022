using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day6Runner : DayRunner
    {
        public Day6Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var buffer = data.Single();

            var firstMarker = FindFirstMarker(buffer, 4);
            OutputWriter.WriteResult(1, $"The first marker is at {firstMarker}");
            
            var firstMessage = FindFirstMarker(buffer, 14);
            OutputWriter.WriteResult(2, $"The first message is at {firstMessage}");
        }

        private static int FindFirstMarker(string buffer, int markerLength)
        {
            for (int i = 0; i+markerLength < buffer.Length; i++)
            {
                if (buffer.Substring(i, markerLength).Distinct().Count() == markerLength)
                {
                    return i + markerLength;
                }
            }

            return -1;
        }
    }
}