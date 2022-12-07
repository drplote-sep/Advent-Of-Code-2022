using System;
using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day7Runner : DayRunner
    {
        public Day7Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var inputs = data.Select(d => new ElfFileSystemInput(d)).ToList();
            var rootDirectory = BuildDirectoryTree(inputs);

            var smallDirectorySum = GetDirectoriesUnderSize(100000, rootDirectory);    
            OutputWriter.WriteResult(1, $"Sum of total sizes of directories of less than 100000: {smallDirectorySum}");

            var smallestDirectorySize = GetSmallestDirectoryToDeleteForUpdate(rootDirectory);
            OutputWriter.WriteResult(2, $"The size of the smallest directory to delete to allow for the update is: {smallestDirectorySize}");
        }

        private long GetSmallestDirectoryToDeleteForUpdate(ElfDirectory rootDirectory)
        {
            long rootDirectorySize = rootDirectory.GetSize();
            long amountOfSpaceNeeded = rootDirectorySize - 40000000;
            if (amountOfSpaceNeeded <= 0)
                return 0;

            return GetSmallestDirectoryToDeleteForUpdate(rootDirectory, amountOfSpaceNeeded);
        }

        private long GetSmallestDirectoryToDeleteForUpdate(ElfDirectory startingDirectory, long spaceNeeded)
        {
            var currentSize = startingDirectory.GetSize();
            if (currentSize >= spaceNeeded)
            {
                var subDirectoryMin = startingDirectory.Contents.OfType<ElfDirectory>()
                    .Where(d => d.GetSize() >= spaceNeeded)
                    .Select(d => GetSmallestDirectoryToDeleteForUpdate(d, spaceNeeded))
                    .DefaultIfEmpty(long.MaxValue)
                    .Min();

                return Math.Min(currentSize, subDirectoryMin);
            }

            return long.MaxValue;
        }
        
        private long GetDirectoriesUnderSize(long maxSize, ElfDirectory startingDirectory)
        {
            long sum = 0;
            if (startingDirectory.GetSize() < maxSize)
            {
                sum += startingDirectory.GetSize();
            }

            sum += startingDirectory.Contents.OfType<ElfDirectory>().Sum(d => GetDirectoriesUnderSize(maxSize, d));

            return sum;
        }

        private ElfDirectory BuildDirectoryTree(List<ElfFileSystemInput> inputs)
        {
            var rootDirectory = new ElfDirectory("/");
            var currentDirectory = rootDirectory;
            
            foreach( var input in inputs)
            {
                switch (input.InputType)
                {
                    case ElfFileSystemInputType.DirectoryItem:
                        currentDirectory.Contents.Add(new ElfDirectory(input.Text, currentDirectory));
                        break;
                    case ElfFileSystemInputType.FileItem:
                        currentDirectory.Contents.Add(new ElfFile(input.Text, input.FileSize.Value, currentDirectory));
                        break;
                    case ElfFileSystemInputType.DownDirectory:
                        currentDirectory =
                            currentDirectory.Contents.OfType<ElfDirectory>().Single(d => d.Name == input.Text);
                        break;
                    case ElfFileSystemInputType.UpDirectory:
                        currentDirectory = currentDirectory.ParentDirectory;
                        break;
                    case ElfFileSystemInputType.RootDirectory:
                        currentDirectory = rootDirectory;
                        break;
                    case ElfFileSystemInputType.ListContents:
                    case ElfFileSystemInputType.None:
                    default:
                        // do nothing
                        break;
                    
                }
            }
            return rootDirectory;
        }
    }

    public enum ElfFileSystemInputType
    {
        None,
        ListContents,
        DownDirectory,
        UpDirectory,
        RootDirectory,
        DirectoryItem,
        FileItem
    }

    public class ElfFileSystemInput
    {
        public string InputString { get; private set; }

        public ElfFileSystemInputType InputType { get; private set; }
        
        public string Text { get; private set; } // File name or directory name 
        public long? FileSize { get; private set; }
        
        
        public ElfFileSystemInput(string input)
        {
            ParseInput(input);
        }

        public void ParseInput(string input)
        {
            InputString = input;
            if (string.IsNullOrWhiteSpace(input))
            {
                InputType = ElfFileSystemInputType.None;
            }
            else
            {
                var parts = input.Split(' ');
                switch (parts[0])
                {
                    case "$":
                        switch (parts[1])
                        {
                            case "cd":
                                switch (parts[2])
                                {
                                    case "/":
                                        InputType = ElfFileSystemInputType.RootDirectory;
                                        break;
                                    case "..":
                                        InputType = ElfFileSystemInputType.UpDirectory;
                                        break;
                                    default:
                                        InputType = ElfFileSystemInputType.DownDirectory;
                                        Text = parts[2];
                                        break;
                                }

                                break;
                            case "ls":
                            {
                                InputType = ElfFileSystemInputType.ListContents;
                                break;
                            }
                            default:
                            {
                                throw new Exception("Unknown command");
                            }
                        }
                        break;
                    case "dir":
                    {
                        InputType = ElfFileSystemInputType.DirectoryItem;
                        Text = parts[1];
                        break;
                    }
                    default:
                    {
                        InputType = ElfFileSystemInputType.FileItem;
                        FileSize = Convert.ToInt64(parts[0]);
                        Text = parts[1];
                        break;
                    }
                }
            }
        }
    }
    
    public abstract class ElfFileSystemItem
    {
        public ElfDirectory ParentDirectory { get; }
        public string Name { get; protected set; }

        protected ElfFileSystemItem(ElfDirectory parentDirectory = null)
        {
            ParentDirectory = parentDirectory;
        }
        
        public abstract long GetSize();
    }

    public class ElfFile : ElfFileSystemItem
    {
        private readonly long _size;
        
        public ElfFile(string name, long size, ElfDirectory parentDirectory) : base(parentDirectory)
        {
            Name = name;
            _size = size;
        }

        public override long GetSize()
        {
            return _size;   
        }
    }
        
    public class ElfDirectory : ElfFileSystemItem
    {
        public readonly List<ElfFileSystemItem> Contents = new List<ElfFileSystemItem>();

        public override long GetSize()
        {
            return Contents.Sum(f => f.GetSize());
        }

        public ElfDirectory(string name, ElfDirectory parentDirectory = null) : base(parentDirectory)
        {
            Name = name;
        }

    }
}