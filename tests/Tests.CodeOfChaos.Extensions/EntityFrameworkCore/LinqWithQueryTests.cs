// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LinqWithQueryTests {
    [Test]
    [Arguments("a", new[] { "a", "b", "c" }, new[] { "a" })]
    [Arguments("b", new[] { "a", "b", "c" }, new[] { "b" })]
    [Arguments("c", new[] { "a", "b", "c" }, new[] { "c" })]
    public async Task ConditionalInclude_ShouldReturnSourceAsIs(string filter, IEnumerable<string> input, IEnumerable<string> expected) {
        // Arrange
        IQueryable<string> source = input.AsQueryable();

        // Act
        IQueryable<string> output = source.With(WhereArg,filter);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
        return;

        IQueryable<string> WhereArg(IQueryable<string> s, string f) => source.Where(x => x == f);
    }
    
    [Test]
    [Arguments("a", "c", new[] { "a", "b", "c" }, new[] { "a", "c" })]
    [Arguments("b", "", new[] { "a", "b", "c" }, new[] { "b" })]
    [Arguments("c", "c", new[] { "a", "b", "c" }, new[] { "c" })]
    public async Task ConditionalInclude_ShouldReturnSourceAsIs(string arg0, string arg1, IEnumerable<string> input, IEnumerable<string> expected) {
        // Arrange
        IQueryable<string> source = input.AsQueryable();

        // Act
        IQueryable<string> output = source.With(WhereArg,arg0, arg1);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
        return;

        IQueryable<string> WhereArg(IQueryable<string> s, string a0, string a1) => source.Where(x => 
            !string.IsNullOrWhiteSpace(a1) && (x == a0 || x == a1) 
            || string.IsNullOrWhiteSpace(a1) && x == a0);
    }
}
