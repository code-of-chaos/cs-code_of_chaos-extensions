// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Running;
using Benchmarks.CodeOfChaos.Extensions.SpanLINQ;

namespace Benchmarks.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main() {
        BenchmarkRunner.Run<InVsScopedVsRefVsRefReadonlyBenchmarks>();
    }
}