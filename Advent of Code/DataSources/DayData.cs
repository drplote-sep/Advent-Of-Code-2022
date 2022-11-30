using System;
using System.Collections.Generic;
using System.IO;

namespace Advent_of_Code.DataSources
{
    public class DayData
    {
        private static readonly string TestData1 = string.Empty;
        
        private Dictionary<int, string[]> TestData { get; } = new Dictionary<int, string[]>
        {
            {
                1, new []{string.Empty}
            }
        };
        
        public DayData(int dayNumber)
        {
            DayNumber = dayNumber;
        }

        public int DayNumber { get; }

        public virtual string[] GetTestData()
        {
            return TestData.ContainsKey(DayNumber) ? TestData[DayNumber] : Array.Empty<string>();
        }

        public virtual string[] GetRealData()
        {
            var path = $"..\\..\\RawInputs\\input{DayNumber}.mos";
            return File.Exists(path) ? File.ReadAllLines(path) : Array.Empty<string>();
        }
    }
}