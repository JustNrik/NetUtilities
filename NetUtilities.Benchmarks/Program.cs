using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace NetUtilities.Benchmarks
{
    class Program
    {

        static void Main()
        {
            BenchmarkRunner.Run<NetUtilitiesBench>();
        }

    }

    public class NetUtilitiesBench
    {
        [Params(10, 100, 1000, 1000)]
        public static int Iterations;

        [Benchmark(Baseline = true, Description = "String Concatenation")]
        public void StringConcat()
        {
            var str = "";
            for (var x = 0; x < Iterations; x++)
                str += "a";
        }

        [Benchmark(Description = "MutableString Concatenation")]
        public void MutableStringConcat()
        {
            var str = new MutableString("");
            for (var x = 0; x < Iterations; x++)
                str += "a";
        }
    }
}
