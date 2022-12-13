using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent_of_Code.Common;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day13Runner : DayRunner
    {
        public Day13Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var pairs = ParsePacketPairs(data);

            var indexSum = 0;
            for (int i = 0; i < pairs.Count; i++)
            {
                var isPairInCorrectOrder = pairs[i].IsPairInCorrectOrder();
                if (isPairInCorrectOrder)
                {
                    indexSum += i + 1;
                }
            }

            OutputWriter.WriteResult(1, $"Sum of indices of pairs in correct order: {indexSum}");

            var allPackets = pairs.Select(p => p.Right).Union(pairs.Select(p => p.Left)).ToList();
            var divider1 = new Packet("[[2]]");
            var divider2 = new Packet("[[6]]");
            allPackets.Add(divider1);
            allPackets.Add(divider2);

            allPackets.Sort();

            var div1Index = allPackets.IndexOf(divider1) + 1;
            var div2Index = allPackets.IndexOf(divider2) + 1;
            OutputWriter.WriteLine("First " + div1Index);
            OutputWriter.WriteLine("Second " + div2Index);
            
            OutputWriter.WriteResult(2, $"Decoder Key: {div1Index * div2Index}");

        }

        private static List<PacketPair> ParsePacketPairs(string[] data)
        {
            var messageGroups = data.GroupByBlankLine();
            List<PacketPair> pairs = new List<PacketPair>();
            foreach (var group in messageGroups)
            {
                pairs.Add(new PacketPair(@group));
            }

            return pairs;
        }

        private static void SanityCheckPacketsBuiltCorrectly(List<PacketPair> pairs)
        {
            foreach (var pair in pairs)
            {
                OutputWriter.WriteLine(pair.ToString());
            }
        }
    }

    public class Packet : IComparable<Packet>
    {
        public PacketData Data { get; }

        public Packet(string s)
        {
            Data = new PacketData();
            var stack = new Stack<PacketData>();
            stack.Push(Data);
            BuildPacketData(s.Skip(1).ToList(), Data, stack);
        }

        public int CompareTo(Packet other)
        {
            if (this == other)
                return 0;
            
            return PacketHelper.Compare(this, other) ? -1 : 1;
        }

        public override string ToString()
        {
            return Data?.ToString() ?? "[]";
        }

        private void BuildPacketData(List<char> input, PacketData current, Stack<PacketData> stack)
        {
            if (!input.Any())
            {
                return; // done
            }

            int skipAmount = 1;

            switch (input[0])
            {
                case '[':
                    var newPacket = new PacketData();
                    current.Data.Add(newPacket);
                    stack.Push(current);
                    current = newPacket;
                    break;
                case ']':
                    current = stack.Pop();
                    break;
                case ',':
                    // do nothing
                    break;
                default:
                    if (char.IsNumber(input[1])) // some numbers are 2 digits. I ain't a proud man.
                    {
                        skipAmount = 2;
                        current.Data.Add(new PacketData { Value = Convert.ToInt32($"{input[0]}{input[1]}") });
                    }
                    else
                    {
                        current.Data.Add(new PacketData { Value = (int)char.GetNumericValue(input[0]) });
                    }

                    break;
            }

            BuildPacketData(input.Skip(skipAmount).ToList(), current, stack);
        }
    }

    public class PacketData
    {
        public bool IsNumeric => Value.HasValue;
        public int? Value { get; set; }
        public List<PacketData> Data { get; set; } = new List<PacketData>();

        public override string ToString()
        {
            if (IsNumeric)
            {
                return Value.ToString();
            }

            return $"[{string.Join(",", Data.Select(d => d.ToString()))}]";
        }
    }

    public class PacketPair
    {
        public Packet Left { get; }
        public Packet Right { get; }

        public PacketPair(List<string> data)
        {
            if (data.Count != 2)
                throw new ArgumentException("Should have two packets", nameof(data));

            Left = new Packet(data[0]);
            Right = new Packet(data[1]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Left?.ToString() ?? "[]");
            sb.AppendLine(Right?.ToString() ?? "[]");
            return sb.ToString();
        }

        public bool IsPairInCorrectOrder()
        {
            return PacketHelper.Compare(Left, Right);
        }
    }

    public class PacketIterator
    {
        public PacketData Current { get; private set; }
        private PacketData _currentList;
        private PacketData _currentData;
        private Stack<PacketData> _dataStack = new Stack<PacketData>();
        private Stack<int> _indexStack = new Stack<int>();
        private int _currentIndex = 0;

        public PacketIterator(Packet packet)
            : this(packet.Data)
        {
        }

        public PacketIterator(PacketData data)
        {
            _currentList = data;
            Next();
        }

        public PacketData NextNumeric()
        {
            PacketData nextVal;
            do
            {
                nextVal = Next();
                if (nextVal != null && !nextVal.IsNumeric && nextVal.Data.Count == 0)
                {
                    nextVal = null;
                    Current = null;
                }
            } while (nextVal != null && !nextVal.IsNumeric );

            return nextVal;
        }

        public void NextIndex()
        {
            _currentIndex++;
            if (_currentIndex >= _currentList.Data.Count)
            {
                PopStack();
            }

            _currentData = _currentList != null && _currentList.Data.Any() ? _currentList.Data[_currentIndex] : null;
            Current = _currentData;
        }

        public PacketData Next()
        {
            Current = null;

            if (_currentList == null || _currentIndex >= _currentList.Data.Count)
                PopStack();

            if (_currentList == null)
                return null;

            PacketData retVal = _currentData;
            if (_currentData == null)
            {
                retVal = _currentList;
                _currentData = _currentList.Data[_currentIndex];
            }
            else if (_currentData.IsNumeric)
            {
                NextIndex();
            }
            else
            {
                PushStack();
                _currentList = _currentData;
                _currentData = _currentList.Data.Any() ? _currentList.Data[_currentIndex] : null;
            }

            Current = retVal;
            return retVal;
        }


        private void PushStack()
        {
            _dataStack.Push(_currentList);
            _indexStack.Push(_currentIndex);
            _currentIndex = 0;
        }

        private void PopStack()
        {
            _currentList = null;
            _currentData = null;
            if (_dataStack.Any())
            {
                _currentIndex = _indexStack.Pop() + 1;
                _currentList = _dataStack.Pop();
            }

            if (_currentList != null && _currentIndex >= _currentList.Data.Count)
            {
                PopStack();
            }
        }
    }

    public static class PacketHelper
    {
        public static bool Compare(Packet leftPacket, Packet rightPacket)
        {
            var left = new PacketIterator(leftPacket);
            var right = new PacketIterator(rightPacket);
            
            while (left.Current != null && right.Current != null)
            {
                var numericComparisonResult = CompareNumerics(left, right);
                if (numericComparisonResult.HasValue)
                {
                    return numericComparisonResult.Value;
                }

                if (left.Current.IsNumeric && !right.Current.IsNumeric)
                {
                    var temp = new PacketIterator(right.Current);
                    temp.NextNumeric();
                    if (temp.Current == null)
                        return false;
                    var result = CompareNumerics(left, temp);
                    if (result.HasValue)
                    {
                        return result.Value;
                    }

                    if (temp.Next() != null)
                    {
                        return true;
                    }
                }

                if (!left.Current.IsNumeric && right.Current.IsNumeric)
                {
                    var temp = new PacketIterator(left.Current);
                    temp.NextNumeric();
                    if (temp.Current == null)
                        return true;
                    var result = CompareNumerics(temp, right);
                    if (result.HasValue)
                    {
                        return result.Value;
                    }

                    if (temp.Next() != null)
                    {
                        return false;
                    }
                }

                if (!left.Current.IsNumeric && !right.Current.IsNumeric)
                {
                    if (left.Current.Data.Any() && !right.Current.Data.Any())
                        return false;

                    if (right.Current.Data.Any() && !left.Current.Data.Any())
                        return true;
                }

                left.Next();
                right.Next();
            }

            return right.Current != null;
        }

        public static bool? CompareNumerics(PacketIterator left, PacketIterator right)
        {
            if (left.Current.IsNumeric && right.Current.IsNumeric)
            {
                if (left.Current.Value < right.Current.Value)
                    return true;
                if (left.Current.Value > right.Current.Value)
                    return false;
            }

            return null;
        }
    }
}