using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day3Runner : DayRunner
    {
        public Day3Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var rucksacks = data.Select(d => new Rucksack(d)).ToList();

            var misplacedItemSum = rucksacks.Sum(r => r.FindCommonItem().Priority);
            OutputWriter.WriteResult(1, $"The sum of priorities of misplaced items is {misplacedItemSum}.");

            var rucksackGroups = new List<RucksackGroup>();
            var remainingRucksacks = rucksacks;
            while (remainingRucksacks.Any())
            {
                rucksackGroups.Add(new RucksackGroup(remainingRucksacks.Take(3).ToArray()));
                remainingRucksacks = remainingRucksacks.Skip(3).ToList();
            }
            
            var badgeItemSum = rucksackGroups.Sum(r => r.GetBadgeItem().Priority);
            OutputWriter.WriteResult(1, $"The sum of priorities of badges is {badgeItemSum}.");
            
        }
    }

    public class RucksackGroup
    {
        private readonly Rucksack[] _rucksacks;

        public RucksackGroup(params Rucksack[] rucksacks)
        {
            _rucksacks = rucksacks;
        }

        public RucksackItem GetBadgeItem()
        {
            var items = _rucksacks.First().GetAllItems();
            foreach (var rucksack in _rucksacks.Skip(1))
            {
                items = items.Intersect(rucksack.GetAllItems()).ToList();
            }

            return items.Single();
        }
    }

    public class Rucksack
    {
        public RucksackCompartment Compartment1 { get;  }
        public RucksackCompartment Compartment2 { get; }

        public List<RucksackItem> GetAllItems()
        {
            var items = new List<RucksackItem>();
            items.AddRange(Compartment1.Contents);
            items.AddRange(Compartment2.Contents);
            return items;
        }
        
        public Rucksack(string contents)
        {
            var length = contents.Length;
            Compartment1 = new RucksackCompartment(contents.Substring(0, length / 2));
            Compartment2 = new RucksackCompartment(contents.Substring(length / 2, length/2));
        }

        public RucksackItem FindCommonItem()
        {
            var item = Compartment1.Contents.Intersect(Compartment2.Contents).Single();
            return item;
        }
    }

    public class RucksackCompartment
    {
        public List<RucksackItem> Contents { get; } = new List<RucksackItem>();
        
        public RucksackCompartment(string contents)
        {
            foreach (char c in contents)
            {
                Contents.Add(new RucksackItem(c));
            }
        }
    }

    public class RucksackItem
    {
        public char Id { get;  }
        public int Priority { get; }

        public RucksackItem(char item)
        {
            Id = item;
            Priority = (int)item - (Char.IsUpper(item) ? 38 : 96);
        }

        public override bool Equals(object obj)
        {
            var otherItem = obj as RucksackItem;
            if (otherItem == null)
                return false;
            
            return Id.Equals(otherItem.Id);
        }

        protected bool Equals(RucksackItem other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    
}