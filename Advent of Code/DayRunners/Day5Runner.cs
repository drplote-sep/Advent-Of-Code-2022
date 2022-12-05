using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day5Runner : DayRunner
    {
        public Day5Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var blankLineIndex = Array.FindIndex(data, d => string.IsNullOrWhiteSpace(d));
            var stackData = data.Take(blankLineIndex).ToList();
            var stacks1 = CreateStacks(stackData);
            var stacks2 = CreateStacks(stackData);
            var instructionData = data.Skip(blankLineIndex + 1).ToList();
            MoveCrates(stacks1, instructionData, true);

            var topCrateIds = string.Join(string.Empty, stacks1.Select(s => s.Pop()));
            OutputWriter.WriteResult(1, $"The crates on the top of each stack spell: {topCrateIds}");
            
            MoveCrates(stacks2, instructionData, false);
            topCrateIds = string.Join(string.Empty, stacks2.Select(s => s.Pop()));
            OutputWriter.WriteResult(2, $"The crates on the top of each stack spell: {topCrateIds}");
        }

        private void MoveCrates(List<Stack<string>> stacks, IEnumerable<string> instructionData, bool oneAtATime)
        {
            foreach (var instruction in instructionData)
            {
                var moves = Regex.Matches(instruction, @"\d+");
                var numCrates = Convert.ToInt32(moves[0].Value);
                var fromStack = Convert.ToInt32(moves[1].Value) - 1;
                var toStack = Convert.ToInt32(moves[2].Value) - 1;

                if (oneAtATime)
                {
                    for (int i = 0; i < numCrates; i++)
                    {
                        stacks[toStack].Push(stacks[fromStack].Pop());
                    }
                }
                else
                {
                    var tempStack = new Stack<string>();
                    for (int i = 0; i < numCrates; i++)
                    {
                        tempStack.Push(stacks[fromStack].Pop());
                    }

                    while (tempStack.Any())
                    {
                        stacks[toStack].Push(tempStack.Pop());
                    }
                }
            }
        }

        private List<Stack<string>> CreateStacks(IEnumerable<string> stackData)
        {
            var reversedStackData = stackData.Reverse().ToList();
            var stackNumbers = Regex.Matches(reversedStackData.First(), @"\d+");
            var maxStackNum = Convert.ToInt32(stackNumbers[stackNumbers.Count - 1].Value);
            var stacks = Enumerable.Range(0, maxStackNum).Select(i => new Stack<string>()).ToList();
            foreach (var stackLine in reversedStackData.Skip(1))
            {
                for (int i = 0; i < stackLine.Length; i = i + 4)
                {
                    var crateId = stackLine[i + 1].ToString();
                    if (!string.IsNullOrWhiteSpace(crateId))
                    {
                        stacks[i / 4].Push(crateId);
                    }
                }
            }
            return stacks;
        }
    }
}