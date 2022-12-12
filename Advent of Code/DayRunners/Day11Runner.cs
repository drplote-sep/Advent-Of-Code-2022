using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent_of_Code.Common;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day11Runner : DayRunner
    {
        public Day11Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            DoPart1(data);
            DoPart2(data);
        }

        private void DoPart2(string[] data)
        {
            var part2Monkeys = ParseMonkeys(data);

            var leastCommonMultiple = AocUtilities.LeastCommonMultiple(part2Monkeys.Values.Select(m => (long)m.Test).ToList());
            foreach (var monkey in part2Monkeys.Values)
            {
                monkey.WorryDecreaseFunc = i => i % leastCommonMultiple;
            }
            
            for (int i = 0; i < 10000; i++)
            {
                DoMonkeyRound(part2Monkeys);
            }

            // foreach (var monkey in part2Monkeys.Values)
            // {
            //     OutputWriter.WriteLine($"Monkey {monkey.MonkeyNumber} inspected items {monkey.InspectionsPerformed} times.");
            // }

            var monkeyBusiness = part2Monkeys.Values.Select(m => m.InspectionsPerformed).OrderByDescending(i => i).Take(2).Aggregate(1L, (x, y) => x * y);
            OutputWriter.WriteResult(2, $"The level of monkey business is: {monkeyBusiness}");
        }

        private void DoPart1(string[] data)
        {
            var part1Monkeys = ParseMonkeys(data);

            for (int i = 0; i < 20; i++)
            {
                DoMonkeyRound(part1Monkeys);
            }

            var monkeyBusiness = part1Monkeys.Values.Select(m => m.InspectionsPerformed).OrderByDescending(i => i).Take(2).Aggregate(1L, (x, y) => x * y);
            OutputWriter.WriteResult(1, $"The level of monkey business is: {monkeyBusiness}");
        }

        private void DoMonkeyRound(Dictionary<int, Monkey> monkeys)
        {
            var maxMonkey = monkeys.Keys.Max();

            for (int i = 0; i <= maxMonkey; i++)
            {
                var currentMonkey = monkeys[i];
                while (currentMonkey.Items.Any())
                {
                    var newWorry = currentMonkey.InspectNextItem();
                    var target = currentMonkey.FindTarget(newWorry);
                    monkeys[target].Items.Enqueue(newWorry);
                }
            }
        }

        private Dictionary<int, Monkey> ParseMonkeys(string[] data)
        {
            var monkeys = new Dictionary<int, Monkey>();
            for (int i = 0; i < data.Length; i += 7)
            {
                var monkeyData = data.Skip(i).Take(7).ToArray();
                var monkey = new Monkey(monkeyData);
                monkeys[monkey.MonkeyNumber] = monkey;
            }

            return monkeys;
        }
    }

    public class Monkey
    {
        public Func<long, long> WorryDecreaseFunc { get; set; } = i => i/3;
        public long InspectionsPerformed { get; private set; } = 0;
        public int MonkeyNumber { get;  }
        public Queue<long> Items { get; } = new Queue<long>();
        public int Test { get; }
        public int TrueMonkeyNumber { get;  }
        public int FalseMonkeyNumber { get;  }
        
        public string OperationType { get; }
        public string OperationAmount { get; }

        public long InspectNextItem()
        {
            InspectionsPerformed++;
            var item = Items.Dequeue();
            var amount = OperationAmount == "old" ? item : Convert.ToInt32(OperationAmount);
            if (OperationType == "*")
            {
                item = item * amount;
            }
            else
            {
                item = item + amount;
            }

            if (item < 0)
                throw new Exception($"{item}");

            return WorryDecreaseFunc(item);
        }
        
        public int FindTarget(long worryLevel)
        {
            return worryLevel % Test == 0 ? TrueMonkeyNumber : FalseMonkeyNumber;
        }

        public Monkey(string[] data)
        {
            MonkeyNumber = data[0].ParseOnlyIntFromString();
            foreach (var item in data[1].ParseAllIntsFromString())
            {
                Items.Enqueue(item);                
            }

            var operationString = data[2].Substring(data[2].IndexOf("new = old") + 9).Trim();
            var opSplit = operationString.Split(' ');
            OperationType = opSplit[0];
            OperationAmount = opSplit[1];
            Test = data[3].ParseOnlyIntFromString();
            TrueMonkeyNumber = data[4].ParseOnlyIntFromString();
            FalseMonkeyNumber = data[5].ParseOnlyIntFromString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Monkey {MonkeyNumber}");
            sb.AppendLine($"  Starting items: {string.Join(", ", Items)}");
            sb.AppendLine($"  Operation: new = old {OperationType} {OperationAmount}");
            sb.AppendLine($"Test: divisible by {Test}");
            sb.AppendLine($"    If true: throw to monkey {TrueMonkeyNumber}");
            sb.AppendLine($"    If false: throw to monkey {FalseMonkeyNumber}");
            sb.AppendLine();
            return sb.ToString();
        }
    }
}