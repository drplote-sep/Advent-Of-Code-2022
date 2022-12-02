using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day2Runner : DayRunner
    {
        public Day2Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var strategies = data.Select(d => new RpsStrategy(d)).ToList();

            var totalAssumedScore = strategies.Select(s => s.GetAssumedScore()).Sum();
            
            OutputWriter.WriteResult(1, $"The total assumed score is {totalAssumedScore}.");

            var totalActualScore = strategies.Select(s => s.GetActualScore()).Sum();
            OutputWriter.WriteResult(2, $"The total actual score is {totalActualScore}");
        }

    }

    public class RpsStrategy
    {
        public RpsThrow Throw { get; private set; }
        public RpsThrow ActualResponse { get; private set; }
        public RpsThrow AssumedResponse { get; private set; }
        public RpsBattleResult ExpectedBattleResult { get; private set; }

        public int GetActualScore()
        {
            return Convert.ToInt32(ActualResponse) + Convert.ToInt32(ExpectedBattleResult);
        }
        
        public int GetAssumedScore()
        {
            return Convert.ToInt32(AssumedResponse) + Convert.ToInt32(CalculateBattleResult(AssumedResponse));
        }

        public int CalculateBattleResult(RpsThrow response)
        {
            if (Throw == response)
                return 3;

            if (response == RpsThrow.Paper && Throw == RpsThrow.Rock ||
                response == RpsThrow.Rock && Throw == RpsThrow.Scissors ||
                response == RpsThrow.Scissors && Throw == RpsThrow.Paper)
            {
                return 6;
            }

            return 0;
        }
        
        public RpsStrategy(string s)
        {
            var pair = s.Split(' ');
            Throw = ParseRpsThrow(pair[0]);
            AssumedResponse = ParseRpsThrow(pair[1]);
            ExpectedBattleResult = ParseBattleResult(pair[1]);
            ActualResponse = DetermineNeededResponseForBattleResult();
        }

        private RpsThrow DetermineNeededResponseForBattleResult()
        {
            if (ExpectedBattleResult == RpsBattleResult.Draw)
            {
                return Throw;
            }

            if (ExpectedBattleResult == RpsBattleResult.Win)
            {
                if (Throw == RpsThrow.Paper) return RpsThrow.Scissors;
                if (Throw == RpsThrow.Rock) return RpsThrow.Paper;
                return RpsThrow.Rock;
            }

            if (Throw == RpsThrow.Paper) return RpsThrow.Rock;
            if (Throw == RpsThrow.Rock) return RpsThrow.Scissors;
            return RpsThrow.Paper;


        }

        private RpsBattleResult ParseBattleResult(string s)
        {
            switch (s)
            {
                case "X":
                    return RpsBattleResult.Lose;
                case "Y":
                    return RpsBattleResult.Draw;
                case "Z":
                    return RpsBattleResult.Win;
                default:
                    throw new ArgumentException("Invalid battle result");
            }
        }
        
        private RpsThrow ParseRpsThrow(string s)
        {
            switch (s)
            {
                case "A":
                case "X":
                    return RpsThrow.Rock;
                case "B":
                case "Y":
                    return RpsThrow.Paper;
                case "C":
                case "Z":
                    return RpsThrow.Scissors; 
                default:
                    throw new ArgumentException("Invalid RPS choice");
            }
        }
    }

    public enum RpsThrow
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public enum RpsBattleResult
    {
        Lose = 0,
        Draw = 3,
        Win = 6
    }
}