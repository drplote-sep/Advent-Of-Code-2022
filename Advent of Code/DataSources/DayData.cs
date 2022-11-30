using System.Collections.Generic;
using System.IO;

namespace Advent_of_Code.DataSources
{
    public class DayData
    {
        private List<string[]> TestData { get; } = new List<string[]>
        {
            new []{
                "199",
                "200",
                "208",
                "210",
                "200",
                "207",
                "240",
                "269",
                "260",
                "263"
            }
        };
        
        public DayData(int dayNumber)
        {
            DayNumber = dayNumber;
        }

        public int DayNumber { get; }

        public virtual string[] GetTestData()
        {
            return TestData[DayNumber - 1];
        }

        public virtual string[] GetRealData()
        {
            return File.ReadAllLines($"..\\..\\RawInputs\\Day {DayNumber}\\input.mos");
        }
    }
}