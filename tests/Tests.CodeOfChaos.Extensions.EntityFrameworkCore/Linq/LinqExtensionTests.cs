// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Tests.CodeOfChaos.Extensions.EntityFrameworkCore.Linq;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(LinqExtensions))]
public class LinqExtensionsTest {
    private readonly List<int> _data = Enumerable.Range(1, 10).ToList();// Data: {1, 2, 3, ... 10}

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    [Arguments(true, new[] { "a", "b", "c" }, new[] { "a", "b", "c" })]
    [Arguments(false, new[] { "a", "b", "c" }, new[] { "a", "b", "c" })]
    public async Task ConditionalInclude_ShouldReturnSourceAsIs(bool condition, IEnumerable<string> input, IEnumerable<string> expected) {
        // Arrange
        IQueryable<string> source = input.AsQueryable();

        // Act
        IQueryable<string> output = source.ConditionalInclude(condition, include: x => x);

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
        IQueryable<string> output = source.ConditionalWhere(condition, predicate: x => x == filterValue);

        // Assert
        await Assert.That(output).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(true, 2, new[] { 1, 2, 3, 4 }, new[] { 1, 2 })]
    [Arguments(true, 3, new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3 })]
    [Arguments(true, 0, new[] { 1, 2, 3, 4 }, new int[] {})]
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
    [Arguments(true, 1, 3, new[] { 10, 20, 30, 40 }, new[] { 20, 30 })]
    [Arguments(false, 2, 5, new[] { 10, 20, 30, 40 }, new[] { 10, 20, 30, 40 })]
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
        IQueryable<int> output = source.ConditionalOrderBy(condition, orderBy: x => x);

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
        IQueryable<string> output = source.ConditionalOrderBy(condition, orderBy: x => x, comparer);

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

    [Test]
    public async Task ConditionalWhere_ConditionTrue_ShouldFilter() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalWhere(true, predicate: x => x > 5).ToList();

        await Assert.That(result).IsEquivalentTo([
            6, 7, 8, 9, 10
        ]);
    }

    [Test]
    public async Task ConditionalWhere_ConditionFalse_ShouldNotFilter() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalWhere(false, predicate: x => x > 5).ToList();

        await Assert.That(result).IsEquivalentTo(_data);
    }

    [Test]
    public async Task ConditionalTake_ConditionTrue_ShouldTakeElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalTake(true, 5).ToList();

        await Assert.That(result).IsEquivalentTo([
            1, 2, 3, 4, 5
        ]);
    }

    [Test]
    public async Task ConditionalTake_ConditionFalse_ShouldNotTakeElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalTake(false, 5).ToList();

        await Assert.That(result).IsEquivalentTo(_data);
    }

    [Test]
    public async Task ConditionalOrderBy_ConditionTrue_ShouldOrder() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalOrderBy(true, orderBy: x => -x).ToList();

        await Assert.That(result).IsEquivalentTo([
            10, 9, 8, 7, 6, 5, 4, 3, 2, 1
        ]);
    }

    [Test]
    public async Task ConditionalOrderBy_ConditionFalse_ShouldNotOrder() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalOrderBy(false, orderBy: x => -x).ToList();

        await Assert.That(result).IsEquivalentTo(_data);
    }

    [Test]
    public async Task ConditionalSkip_ConditionTrue_ShouldSkipElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalSkip(true, 5).ToList();

        await Assert.That(result).IsEquivalentTo([
            6, 7, 8, 9, 10
        ]);
    }

    [Test]
    public async Task ConditionalSkip_ConditionFalse_ShouldNotSkipElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalSkip(false, 5).ToList();

        await Assert.That(result).IsEquivalentTo(_data);
    }

    [Test]
    public async Task ConditionalDistinct_ConditionTrue_ShouldReturnDistinct() {
        IQueryable<int> source = new List<int> { 1, 2, 2, 3, 3, 3 }.AsQueryable();
        List<int> result = source.ConditionalDistinct(true).ToList();

        await Assert.That(result).IsEquivalentTo([
            1, 2, 3
        ]);
    }

    [Test]
    public async Task ConditionalDistinct_ConditionFalse_ShouldReturnOriginal() {
        IQueryable<int> source = new List<int> { 1, 2, 2, 3, 3, 3 }.AsQueryable();
        List<int> result = source.ConditionalDistinct(false).ToList();

        await Assert.That(result).IsEquivalentTo([
            1, 2, 2, 3, 3, 3
        ]);
    }

    [Test]
    public async Task ConditionalUnion_ConditionTrue_ShouldUnion() {
        IQueryable<int> source = new List<int> { 1, 2, 3 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalUnion(true, second).ToList();

        await Assert.That(result).IsEquivalentTo([
            1, 2, 3, 4, 5
        ]);
    }

    [Test]
    public async Task ConditionalUnion_ConditionFalse_ShouldNotUnion() {
        IQueryable<int> source = new List<int> { 1, 2, 3 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalUnion(false, second).ToList();

        await Assert.That(result).IsEquivalentTo([
            1, 2, 3
        ]);
    }

    [Test]
    public async Task ConditionalExcept_ConditionTrue_ShouldApplyExcept() {
        IQueryable<int> source = new List<int> { 1, 2, 3, 4 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalExcept(true, second).ToList();

        await Assert.That(result).IsEquivalentTo([
            1, 2
        ]);
    }

    [Test]
    public async Task ConditionalExcept_ConditionFalse_ShouldNotApplyExcept() {
        IQueryable<int> source = new List<int> { 1, 2, 3, 4 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalExcept(false, second).ToList();

        await Assert.That(result).IsEquivalentTo([
            1, 2, 3, 4
        ]);
    }
}
