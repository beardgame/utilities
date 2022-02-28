using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Bearded.Utilities.Benchmarks;

static class Program
{
    static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
    }
}
