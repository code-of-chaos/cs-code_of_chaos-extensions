// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Runtime.CompilerServices;

namespace Benchmarks.CodeOfChaos.Extensions.SpanLINQ;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class InVsScopedVsRefVsRefReadonlyBenchmarks {
    // Parameter for data sizes
    [Params(10, 1000, 100000)]
    public int DataSize { get; set; }

    // Parameter for match scenarios
    [ParamsSource(nameof(MatchScenarios))]
    public MatchScenario Scenario { get; set; }

    private int[] _data;

    [GlobalSetup]
    public void Setup() {
        _data = Enumerable.Range(1, DataSize).ToArray();
    }

    public static IEnumerable<MatchScenario> MatchScenarios => new[] {
        new MatchScenario("NoMatch", x => x < 0),
        new MatchScenario("MiddleMatch", x => x == 500),
        new MatchScenario("ImmediateMatch", x => x > 0)
    };

    public record MatchScenario(string Name, Func<int, bool> Predicate) {
        public override string ToString() => Name;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool AnyScoped<T>(scoped ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (predicate(span[i]))
                return true;

        return false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool AnyIn<T>(in ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (predicate(span[i]))
                return true;

        return false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool AnyRef<T>(ref ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (predicate(span[i]))
                return true;

        return false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool AnyRefReadonly<T>(ref readonly ReadOnlySpan<T> span, Func<T, bool> predicate) {
        for (int i = span.Length - 1; i >= 0; i--)
            if (predicate(span[i]))
                return true;

        return false;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    [Benchmark]
    public bool Bench_Scoped() {
        ReadOnlySpan<int> span = _data;
        return AnyScoped(span, Scenario.Predicate);
    }

    [Benchmark]
    public bool Bench_In() {
        ReadOnlySpan<int> span = _data;
        return AnyIn(span, Scenario.Predicate);
    }

    [Benchmark]
    public bool Bench_Ref() {
        ReadOnlySpan<int> span = _data;
        return AnyRef(ref span, Scenario.Predicate);
    }

    [Benchmark]
    public bool Bench_RefReadOnly() {
        ReadOnlySpan<int> span = _data;
        return AnyRefReadonly(ref span, Scenario.Predicate);
    }
}
