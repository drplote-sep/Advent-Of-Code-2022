using System;

namespace Advent_of_Code
{
    public static class OutputWriter
    {
        private static readonly int DefaultOffset = 4;

        public static void WriteTimeResult(long elapsedMilliseconds)
        {
            Console.WriteLine();
            WriteLine($"Elapsed time: {elapsedMilliseconds} ms", DefaultOffset);
            WriteSeparator();
            Console.WriteLine();
        }

        public static void WriteResult(int partNum, string s)
        {
            WriteLine($"Part {partNum}: {s}", DefaultOffset);
        }

        public static void WriteDayHeader(int dayNum)
        {
            WriteHeader($"Day {dayNum} Results");
        }

        public static void WriteHeader(string s)
        {
            WriteLine(s);
            WriteSeparator();
        }

        public static void WriteSeparator(int offset = 0)
        {
            WriteLine("------------------", offset);
        }

        public static void WriteLine(string s, int offset = 0)
        {
            var whitespace = new string(' ', offset);
            Console.WriteLine($"{whitespace}{s}");
        }

    }
}