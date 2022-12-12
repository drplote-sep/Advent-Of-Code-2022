using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day12Runner : DayRunner
    {
        public Day12Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var fewestSteps = GetShortestDistanceFromStart(data);
            OutputWriter.WriteResult(1, $"The fewest steps is: {fewestSteps}");

            var fewestStepsFromAny = GetShortestDistanceFromAnyA(data);
            OutputWriter.WriteResult(2, $"The fewest steps from any 'a' is: {fewestStepsFromAny}");
        }

        private int GetShortestDistanceFromAnyA(string[] data)
        {
            var starts = FindLocation(data, "a").Select(p => new Tile(p)).ToList();
            starts.Add(new Tile(FindLocation(data, "S").Single()));
            var finish = new Tile(FindLocation(data, "E").Single());
            
            foreach (var start in starts)
            {
                start.SetDistance(finish.X, finish.Y);
            }

            return GetShortestDistance(data, starts, finish);
        }
        
        private int GetShortestDistanceFromStart(string[] data)
        {
            var start = new Tile(FindLocation(data, "S").Single());
            var finish = new Tile(FindLocation(data, "E").Single());

            start.SetDistance(finish.X, finish.Y);

            var activeTiles = new List<Tile> { start };

            return GetShortestDistance(data, activeTiles, finish);
        }

        private static int GetShortestDistance(string[] data, List<Tile> activeTiles, Tile finish)
        {
            var visitedTiles = new List<Tile>();
            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();
                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    return checkTile.GetDistanceWalked();
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(data, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    {
                        var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            return int.MaxValue;
        }

        private static List<Tile> GetWalkableTiles(string[] data, Tile currentTile, Tile targetTile)
        {
            var possibleTiles = new List<Tile>()
            {
                new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            };
            
            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

            var maxX = data[0].Length - 1;
            var maxY = data.Length - 1;

            var currentElevation = data[currentTile.Y][currentTile.X];
            return possibleTiles
                .Where(tile => tile.X >= 0 && tile.X <= maxX)
                .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
                .Where(tile => CanClimb(currentElevation, data[tile.Y][tile.X]))
                .ToList();
        }

        private static bool CanClimb(char currentElevation, char targetElevation)
        {
            if (currentElevation == 'S') currentElevation = 'a';
            if (targetElevation == 'E') targetElevation = 'z';
            
            return currentElevation >= targetElevation || 
                   currentElevation + 1 == targetElevation;
        }

        private IEnumerable<Point> FindLocation(string[] data, string location)
        {
            var locations = new List<Point>();
            for (int y = 0; y < data.Length; y++)
            {
                var index = data[y].IndexOf(location);
                if (index != -1)
                {
                    locations.Add(new Point(index, y));
                }
            }

            return locations;
        }

    }

    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Tile Parent { get; set; }

        public int GetDistanceWalked()
        {
            return 1 + Parent?.GetDistanceWalked() ?? 0;
        }

        public Tile()
        {
        }
        
        public Tile(Point position)
        {
            X = position.X;
            Y = position.Y;
        }

        public void SetDistance(int targetX, int targetY)
        {
            Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }
        
    }
}