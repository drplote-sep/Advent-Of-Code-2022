using System;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day4Runner : DayRunner
    {
        public Day4Runner(DayData data) : base(data)
        {
            
        }

        protected override void SolveDay(string[] data)
        {
            var cleaningPairs = data.Select(d => new CleaningPair(d)).ToList();

            var numContainedPairs = cleaningPairs.Count(p => p.HasFullyContainedSection());
            OutputWriter.WriteResult(1, $"{numContainedPairs} have one range fully containing the other.");
            
            var numOverlappingPairs = cleaningPairs.Count(p => p.HasOverlappingSections());
            OutputWriter.WriteResult(1, $"{numOverlappingPairs} have have ranges that overlap.");
        }
    }

    public class CleaningPair
    {
        public CleaningArea Section1 { get; }
        public CleaningArea Section2 { get; }

        public CleaningPair(string s)
        {
            var parts = s.Split(',');
            Section1 = new CleaningArea(parts[0]);
            Section2 = new CleaningArea(parts[1]);
        }

        public bool HasFullyContainedSection()
        {
            return Section1.ContainsArea(Section2) || Section2.ContainsArea(Section1);
        }

        public bool HasOverlappingSections()
        {
            return Section1.HasOverlap(Section2);
        }
    }

    public class CleaningArea
    {
        public int StartingSectionId { get; }
        public int EndingSectionId { get; }

        public CleaningArea(int starting, int ending)
        {
            StartingSectionId = starting;
            EndingSectionId = ending;
        }

        public CleaningArea(string s)
        {
            var parts = s.Split('-');
            StartingSectionId = Convert.ToInt32(parts[0]);
            EndingSectionId = Convert.ToInt32(parts[1]);
        }

        public bool ContainsArea(CleaningArea other)
        {
            return other.StartingSectionId >= StartingSectionId && other.EndingSectionId <= EndingSectionId;
        }

        public bool HasOverlap(CleaningArea other)
        {
            var thisRange = Enumerable.Range(
                StartingSectionId, 
                EndingSectionId - StartingSectionId + 1);
            var otherRange = Enumerable.Range(
                other.StartingSectionId, 
                other.EndingSectionId - other.StartingSectionId + 1);

            return thisRange.Intersect(otherRange).Any();
        }
    }
}