using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent_of_Code.Common
{
    public static class AocUtilities
    {
        public static List<int> ParseAllIntsFromString(this string s)
        {
            var ints = new List<int>();
            foreach (Match match in Regex.Matches(s, @"\d+"))
            {
                ints.Add(Convert.ToInt32(match.Value));
            }
            return ints;
        }
        
        public static int ParseOnlyIntFromString(this string s)
        {
            var match = Regex.Match(s, @"\d+");
            return Convert.ToInt32(match.Value);
        }
        
        public static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static long LeastCommonMultiple(long a, long b)
        {
            return (a / GreatestCommonFactor(a, b)) * b;
        }

        public static long LeastCommonMultiple(List<long> nums)
        {
            if (nums.Count == 1)
            {
                return nums[0];
            }
            if (nums.Count == 2)
            {
                return LeastCommonMultiple(nums[0], nums[1]);
            }

            return LeastCommonMultiple(nums.First(), LeastCommonMultiple(nums.Skip(1).ToList()));
        }
    }
}