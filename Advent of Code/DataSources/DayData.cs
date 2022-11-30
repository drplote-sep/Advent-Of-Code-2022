using System;
using System.Collections.Generic;
using System.IO;

namespace Advent_of_Code.DataSources
{
    public class DayData
    {
        public DayData(int dayNumber)
        {
            DayNumber = dayNumber;
        }

        public int DayNumber { get; }

        public virtual string[] GetTestData()
        {
            var path = $"..\\..\\TestData\\test{DayNumber}.mos";
            return File.Exists(path) ? File.ReadAllLines(path) : Array.Empty<string>();
        }

        public virtual string[] GetRealData()
        {
            var path = $"..\\..\\RealData\\input{DayNumber}.mos";
            return File.Exists(path) ? File.ReadAllLines(path) : Array.Empty<string>();
        }
    }
}