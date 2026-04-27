// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

namespace CodeOfChaosTests.Extensions.EntityFrameworkCore.Linq;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LinqWithQueryTests {
    private static readonly string[] Input = ["a", "b", "c"];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<(string, string[])>> ConditionalInclude_ShouldReturnSourceAsIs_GetTestCases() {
        yield return () => ("a", ["a"]);
        yield return () => ("b", ["b"]);
        yield return () => ("c", ["c"]);
    }
    
    [Test]
    [MethodDataSource(nameof(ConditionalInclude_ShouldReturnSourceAsIs_GetTestCases))]
    public async Task ConditionalInclude_ShouldReturnSourceAsIs(string filter, params string[] expected) {
        // Arrange
        IQueryable<string> source = Input.AsQueryable();

        // Act
        IQueryable<string> output = source.With(WhereArg, filter);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
        return;

        IQueryable<string> WhereArg(IQueryable<string> s, string f) {
            return source.Where(x => x == f);
        }
    }

    public static IEnumerable<Func<(string, string, string[])>> ConditionalInclude_2Args_ShouldReturnSourceAsIs_GetTestCases() {
        yield return () => ("a", "c", ["a", "c"]);
        yield return () => ("b", "", ["b"]);
        yield return () => ("c", "c", ["c"]);
    }

    [Test]
    [MethodDataSource(nameof(ConditionalInclude_2Args_ShouldReturnSourceAsIs_GetTestCases))]
    public async Task ConditionalInclude_2Args_ShouldReturnSourceAsIs(string arg0, string arg1, string[] expected) {
        // Arrange
        IQueryable<string> source = Input.AsQueryable();

        // Act
        IQueryable<string> output = source.With(WhereArg, arg0, arg1);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
        return;

        IQueryable<string> WhereArg(IQueryable<string> s, string a0, string a1) {
            return source.Where(x =>
                !string.IsNullOrWhiteSpace(a1) && (x == a0 || x == a1)
                || string.IsNullOrWhiteSpace(a1) && x == a0);
        }
    }
}
