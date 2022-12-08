using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day8Runner : DayRunner
    {
        public Day8Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var treeGrid = new TreeGrid(data);

            var numVisible = treeGrid.GetNumberOfVisibleTrees();
            OutputWriter.WriteResult(1, $"There are {numVisible} visible trees.");

            var bestScore = treeGrid.GetBestScenicScore();
            OutputWriter.WriteResult(2, $"The best scenic score is {bestScore}.");

        }
    }


    public class TreeGrid
    {
        private int Size { get; }
        private Dictionary<Point, int> HeightGrid { get; } = new Dictionary<Point, int>();
        private Dictionary<int, List<int>> Rows { get; } = new Dictionary<int, List<int>>();
        private Dictionary<int, List<int>> Columns { get; } = new Dictionary<int, List<int>>();

        public TreeGrid(string[] data)
        {
            Size = data.Length;
            PopulateGrid(data);
        }

        private void PopulateGrid(string[] data)
        {
            for(int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    HeightGrid[new Point(x, y)] = Convert.ToInt32(char.GetNumericValue(data[y][x]));
                }
            }
        }

        public List<int> GetRow(int y)
        {
            if (!Rows.ContainsKey(y))
            {
                Rows[y] = HeightGrid
                    .Where(d => d.Key.Y == y)
                    .OrderBy(d => d.Key.X)
                    .Select(d => d.Value)
                    .ToList();
            }

            return Rows[y];
        }

        public List<int> GetColumn(int x)
        {
            if (!Columns.ContainsKey(x))
            {
                Columns[x] = HeightGrid
                    .Where(d => d.Key.X == x)
                    .OrderBy(d => d.Key.Y)
                    .Select(d => d.Value)
                    .ToList();
            }

            return Columns[x];
        }

        private bool IsPointVisible(Point point)
        {
            if (point.X == 0 || point.Y == 0 || 
                point.X == Size - 1 || point.Y == Size -1)
            {
                return true;
            }

            var height = HeightGrid[point];
            var row = GetRow(point.Y);
            var column = GetColumn(point.X);
            var rowLeft = row.Take(point.X).ToList();
            var rowRight = row.Skip(point.X + 1).ToList();
            var columnUp = column.Take(point.Y).ToList();
            var columnDown = column.Skip(point.Y + 1).ToList();

            return rowLeft.Max() < height  ||
                   rowRight.Max() < height ||
                   columnUp.Max() < height ||
                   columnDown.Max() < height;
        }
        private int GetScenicScore(Point point)
        {
            var height = HeightGrid[point];
            var row = GetRow(point.Y);
            var column = GetColumn(point.X);
            var rowLeft = row.Take(point.X).Reverse().ToList();
            var rowRight = row.Skip(point.X + 1).ToList();
            var columnUp = column.Take(point.Y).Reverse().ToList();
            var columnDown = column.Skip(point.Y + 1).ToList();

            return CountUntilBlocked(height, rowLeft) *
                   CountUntilBlocked(height, rowRight) *
                   CountUntilBlocked(height, columnUp) *
                   CountUntilBlocked(height, columnDown);
        }

        private int CountUntilBlocked(int maxHeight, List<int> candidates)
        {
            if (candidates.Any())
            {
                if (candidates[0] >= maxHeight)
                    return 1;

                return 1 + CountUntilBlocked(maxHeight, candidates.Skip(1).ToList());
            }

            return 0;
        }
        
        public int GetBestScenicScore()
        {
            return HeightGrid.Keys.Select(point => GetScenicScore(point)).Max();
        }

        public int GetNumberOfVisibleTrees()
        {
            var visiblePoints = new List<Point>();
            foreach (var point in HeightGrid.Keys)
            {
                if (IsPointVisible(point))
                {
                    visiblePoints.Add(point);
                }
            }

            return visiblePoints.Count;
        }
    }
}