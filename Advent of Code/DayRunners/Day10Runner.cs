using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day10Runner : DayRunner
    {
        public Day10Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var clock = new ClockCircuit();
            clock.ExecuteInstructions(data);
            var sumOfSignalStrengths = clock.GetSignalStrength(20) +
                                       clock.GetSignalStrength(60) +
                                       clock.GetSignalStrength(100) +
                                       clock.GetSignalStrength(140) +
                                       clock.GetSignalStrength(180) +
                                       clock.GetSignalStrength(220);
            OutputWriter.WriteResult(1, $"The sum of the six signal strengths is: {sumOfSignalStrengths}");
            
            OutputWriter.WriteResult(2, $"The rendered image is: {System.Environment.NewLine + clock.RenderImage()}");
        }
        
        
    }

    public class ClockCircuit
    {
        public Dictionary<int, int> RegisterValues = new Dictionary<int, int>();
        public int CurrentCycle { get; private set; } = 0;
        public int X { get; private set; } = 1;

        public Dictionary<int, int> UpcomingXChanges = new Dictionary<int, int> { [1] = 0, [2] = 0 };

        public int GetSignalStrength(int cycleNumber)
        {
            return RegisterValues[cycleNumber] * cycleNumber;
        }

        public void ExecuteInstructions(string[] data)
        {
            foreach (var instruction in data)
            {
                var splitInstruction = instruction.Split(' ');
                if (splitInstruction.Length == 2)
                {
                    AddRegisterInstruction(Convert.ToInt32(splitInstruction[1]));
                    Cycle();
                    Cycle();
                }
                else
                {
                    Cycle();
                }
            }
        }

        private void AddRegisterInstruction(int x)
        {
            UpcomingXChanges[2] = x;
        }

        private void Cycle()
        {
            CurrentCycle++;
            RegisterValues[CurrentCycle] = X;
            X += UpcomingXChanges[1];
            UpcomingXChanges[1] = UpcomingXChanges[2];
            UpcomingXChanges[2] = 0;
        }

        public string RenderImage()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    sb.Append(GetPixel(i, j));
                }

                sb.AppendLine();
            }

            return sb.ToString();

        }

        private string GetPixel(int screenRow, int screenColumn)
        {
            var cycle = screenRow * 40 + screenColumn + 1;
            var pixelPosition = RegisterValues[cycle];
            return Math.Abs(pixelPosition - screenColumn) < 2 ? "#" : ".";
        }
    }
}