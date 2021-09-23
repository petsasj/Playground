using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Playground.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkClass>();
        }
    }

    [MemoryDiagnoser]
    public class BenchmarkClass
    {
        [Benchmark]
        public async Task<int> TestTask()
        {
            return 1;
        }
    }
}