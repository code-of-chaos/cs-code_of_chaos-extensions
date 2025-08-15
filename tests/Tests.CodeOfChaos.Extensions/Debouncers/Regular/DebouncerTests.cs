// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;

namespace Tests.CodeOfChaos.Extensions.Debouncers.Regular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DebouncerTests {
    public static IEnumerable<Func<(IDebouncerBase, bool)>> GetEmptyDebouncers() {
        yield return () => (Debouncer.Empty, true);
        yield return () => (Debouncer<string>.Empty, true);
        yield return () => (Debouncer<int>.Empty, true);
        yield return () => (Debouncer.FromDelegate(() => {}), false);
        yield return () => (Debouncer<string>.FromDelegate(_ => {}), false);
        yield return () => (Debouncer<int>.FromDelegate(_ => {}), false);
        
        yield return () => (ThrottledDebouncer.Empty, true);
        yield return () => (ThrottledDebouncer<string>.Empty, true);
        yield return () => (ThrottledDebouncer<int>.Empty, true);
        yield return () => (ThrottledDebouncer.FromDelegate(() => {}), false);
        yield return () => (ThrottledDebouncer<string>.FromDelegate(_ => {}), false);
        yield return () => (ThrottledDebouncer<int>.FromDelegate(_ => {}), false);
    }
    
    [Test]
    [MethodDataSource(nameof(GetEmptyDebouncers))]
    public async Task Empty_ShouldBeEmpty(IDebouncerBase debouncer, bool expectedIsEmpty) {
        // Arrange & Act
        bool result = debouncer.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(expectedIsEmpty);
    }
}
