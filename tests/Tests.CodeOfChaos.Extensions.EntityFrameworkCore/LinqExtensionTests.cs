// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(LinqExtensions))]
public class LinqExtensionsTest {

    [Test]
    [Arguments(true, new[] { "a", "b", "c" }, new[] { "a", "b", "c" })]
    [Arguments(false, new[] { "a", "b", "c" }, new[] { "a", "b", "c" })]
    public async Task ConditionalInclude_ShouldReturnSourceAsIs(bool condition, IEnumerable<string> input, IEnumerable<string> expected) {
        // Arrange
        IQueryable<string> source = input.AsQueryable();

        // Act
        IQueryable<string> output = source.ConditionalInclude(condition, x => x);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(true, "a", new[] { "a", "b", "a" }, new[] { "a", "a" })]
    [Arguments(false, "a", new[] { "a", "b", "a" }, new[] { "a", "b", "a" })]
    public async Task ConditionalWhere_ShouldFilterIfConditionIsTrue(bool condition, string filterValue, IEnumerable<string> input, IEnumerable<string> expected) {
        // Arrange
        IQueryable<string> source = input.AsQueryable();

        // Act
        IQueryable<string> output = source.ConditionalWhere(condition, x => x == filterValue);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(true, 2, new[] { 1, 2, 3, 4 }, new[] { 1, 2 })]
    [Arguments(true, 3, new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3 })]
    [Arguments(true, 0, new[] { 1, 2, 3, 4 }, new int[] { })]
    [Arguments(false, 2, new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
    [Arguments(false, 3, new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
    [Arguments(false, 0, new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
    public async Task ConditionalTake_WithCount_ShouldReturnCorrectSubset(bool condition, int count, IEnumerable<int> input, IEnumerable<int> expected) {
        // Arrange
        IQueryable<int> source = input.AsQueryable();

        // Act
        IQueryable<int> output = source.ConditionalTake(condition, count);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(true, 1,3, new[] { 10, 20, 30, 40 }, new[] { 20, 30 })]
    [Arguments(false, 2,5, new[] { 10, 20, 30, 40 }, new[] { 10, 20, 30, 40 })]
    public async Task ConditionalTake_WithRange_ShouldReturnCorrectSubset(bool condition, int rangeStart, int rangeEnd, IEnumerable<int> input, IEnumerable<int> expected) {
        // Arrange
        IQueryable<int> source = input.AsQueryable();

        // Act
        IQueryable<int> output = source.ConditionalTake(condition, new Range(rangeStart, rangeEnd));

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(true, new[] { 5, 3, 8 }, new[] { 3, 5, 8 })]
    [Arguments(false, new[] { 5, 3, 8 }, new[] { 5, 3, 8 })]
    public async Task ConditionalOrderBy_ShouldOrderCorrectly(bool condition, IEnumerable<int> input, IEnumerable<int> expected) {
        // Arrange
        IQueryable<int> source = input.AsQueryable();

        // Act
        IQueryable<int> output = source.ConditionalOrderBy(condition, x => x);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(true, new[] { "b", "a", "c" }, null, new[] { "a", "b", "c" })]
    [Arguments(false, new[] { "b", "a", "c" }, null, new[] { "b", "a", "c" })]
    public async Task ConditionalOrderByWithComparer_ShouldOrderCorrectlyWithComparer(bool condition, IEnumerable<string> input, IComparer<string>? comparer, IEnumerable<string> expected) {
        // Arrange
        IQueryable<string> source = input.AsQueryable();
        comparer ??= StringComparer.Ordinal;

        // Act
        IQueryable<string> output = source.ConditionalOrderBy(condition, x => x, comparer);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(true, new[] { 5, 2, 8 }, new[] { 2, 5, 8 })]
    [Arguments(false, new[] { 5, 2, 8 }, new[] { 5, 2, 8 })]
    public async Task ConditionalOrderByNotNull_WithKeySelector_ShouldSortCorrectly(bool condition, IEnumerable<int> input, IEnumerable<int> expected) {
        // Arrange
        Expression<Func<int, object>>? orderBy = condition ? x => x : null;
        IQueryable<int> source = input.AsQueryable();

        // Act
        IQueryable<int> output = source.ConditionalOrderByNotNull(orderBy);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }
}