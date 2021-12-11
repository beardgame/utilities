using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Bearded.Utilities.Linq;
using BenchmarkDotNet.Attributes;

namespace Bearded.Utilities.Benchmarks.Linq;

[SuppressMessage("ReSharper", "ClassCanBeSealed.Global")]
public sealed class Extensions
{
    private const int seed = 1337;
    private const int count = 10000;

    public abstract class ListBenchmark
    {
        protected List<int> List { get; }
        protected Random Random { get; }

        protected ListBenchmark()
        {
            List = new List<int>(count);
            Random = new Random(seed);
            for (var i = 0; i < count; i++)
            {
                List.Add(Random.Next());
            }
        }
    }

    public class RandomElement : ListBenchmark
    {
        [Benchmark]
        public int GetRandomElement()
        {
            return List.RandomElement();
        }
    }

    public class RandomSubset : ListBenchmark
    {
        [Params(1, 100, count / 2, count - 100, count)]
        public int SubsetSize { get; set; }

        [Benchmark]
        public List<int> GetRandomSubset()
        {
            return List.RandomSubset(SubsetSize);
        }
    }

    public class Shuffle : ListBenchmark
    {
        [Benchmark]
        public void ShuffleInPlace()
        {
            List.Shuffle(Random);
        }

        [Benchmark]
        public IList<int> Shuffled()
        {
            return List.Shuffled(Random);
        }
    }
}
