// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Running;
using CodeOfChaosBenchmarks.Extensions.SpanLINQ;

namespace CodeOfChaosBenchmarks.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main() {
        BenchmarkRunner.Run<InVsScopedVsRefVsRefReadonlyBenchmarks>();
    }
}