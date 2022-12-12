using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day9Runner : DayRunner
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
        
        public Day9Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var ropePath = MoveRope(data, 2);
            var numTailPositions = ropePath.Select(p => p.Tail).Distinct().Count();
            
            OutputWriter.WriteResult(1, $"Unique Positions tail visits: {numTailPositions}");
            
            var biggerRopePath = MoveRope(data, 10);
            var numTailPositionsBigger = biggerRopePath.Select(p => p.Tail).Distinct().Count();
            
            OutputWriter.WriteResult(2, $"Unique Positions tail visits (with 10 knots): {numTailPositionsBigger}");
        }

        private List<RopePosition> MoveRope(string[] data, int numKnots)
        {
            List<RopePosition> positions = new List<RopePosition> { new RopePosition(numKnots) };

            foreach (var instruction in data)
            {
                RunInstruction(instruction, positions);
            }
            
            return positions;
        }

        private Direction ParseDirection(string input)
        {
            switch (input)
            {
                case "R":
                    return Direction.Right;
                case "L":
                    return Direction.Left;
                case "U":
                    return Direction.Up;
                case "D":
                    return Direction.Down;
                default:
                    throw new Exception($"Unknown direction {input}");
            }
        }

        private void RunInstruction(string data, List<RopePosition> positions)
        {
            int numKnots = positions[0].Knots.Count;
            
            var instructionSplit = data.Split(' ');
            var direction = ParseDirection(instructionSplit[0]);
            var amount = Convert.ToInt32(instructionSplit[1]);

            for (int i = amount; i > 0; i--)
            {
                var newPosition = new RopePosition();
                var previousPosition = positions.Last();
                Point newKnot = Point.Empty;
                
                for (int knotNum = 0; knotNum < numKnots; knotNum++)
                {
                    if (knotNum == 0)
                    {
                        newKnot = GetNewHeadPosition(previousPosition.Head, direction);
                    }
                    else
                    {
                        newKnot =GetNewTailPosition(previousPosition.Knots[knotNum], newPosition.Knots.Last());

                    }
                    newPosition.Knots.Add(newKnot);
                }
                positions.Add(newPosition);
            }
        }

        private bool AreTouching(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) <= 1 && Math.Abs(p1.Y - p2.Y) <= 1;
        }
        
        private Point GetNewTailPosition(Point currentPosition, Point headPosition)
        {
            if (AreTouching(currentPosition, headPosition))
            {
                return currentPosition;
            }

            var newPosition = new Point(currentPosition.X, currentPosition.Y);
            
            int xDifference = Math.Abs(headPosition.X - currentPosition.X);
            int yDifference = Math.Abs(headPosition.Y - currentPosition.Y);

            if (xDifference > 1 || xDifference == 1 && yDifference > 1)
            {
                if (currentPosition.X < headPosition.X)
                {
                    newPosition.X += 1;
                }
                else
                {
                    newPosition.X -= 1;    
                }
            }

            if (yDifference > 1 || yDifference == 1 && xDifference > 1)
            {
                if( currentPosition.Y < headPosition.Y)
                {
                    newPosition.Y += 1;
                }
                else
                {
                    newPosition.Y -= 1;    
                }
            }

            return newPosition;
        }

        private Point GetNewHeadPosition(Point currentPosition, Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return new Point(currentPosition.X, currentPosition.Y - 1);
                case Direction.Up:
                    return new Point(currentPosition.X, currentPosition.Y + 1);
                case Direction.Right:
                    return new Point(currentPosition.X + 1, currentPosition.Y);
                case Direction.Left:
                    return new Point(currentPosition.X - 1, currentPosition.Y);
                default:
                    throw new Exception($"Invalid direction: {direction}");
            }
        }
    }
    
    public class RopePosition
    {
        public List<Point> Knots { get; } = new List<Point>();
        public Point Head => Knots.First();
        public Point Tail => Knots.Last();

        public RopePosition(int numKnots)
        {
            for (int i = 0; i < numKnots; i++)
            {
                Knots.Add(new Point(0, 0));
            }
        }
        
        public RopePosition(params Point[] points)
        {
            Knots = points.ToList();
        }
    }
}