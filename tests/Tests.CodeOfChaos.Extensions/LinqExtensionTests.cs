// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using System.Linq.Expressions;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace Tests.CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(LinqExtensions))]
public class LinqExtensionsTests {
    private readonly List<int> _data = Enumerable.Range(1, 10).ToList();// Data: {1, 2, 3, ... 10}

    [Test]
    public async Task ConditionalWhere_ConditionTrue_ShouldFilter() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalWhere(true, predicate: x => x > 5).ToList();

        await Assert.That(result).IsEqualTo([
            6, 7, 8, 9, 10
        ]);
    }

    [Test]
    public async Task ConditionalWhere_ConditionFalse_ShouldNotFilter() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalWhere(false, predicate: x => x > 5).ToList();

        await Assert.That(result).IsEqualTo(_data);
    }

    [Test]
    public async Task ConditionalTake_ConditionTrue_ShouldTakeElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalTake(true, 5).ToList();

        await Assert.That(result).IsEqualTo([
            1, 2, 3, 4, 5
        ]);
    }

    [Test]
    public async Task ConditionalTake_ConditionFalse_ShouldNotTakeElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalTake(false, 5).ToList();

        await Assert.That(result).IsEqualTo(_data);
    }

    [Test]
    public async Task ConditionalOrderBy_ConditionTrue_ShouldOrder() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalOrderBy(true, orderBy: x => -x).ToList();

        await Assert.That(result).IsEqualTo([
            10, 9, 8, 7, 6, 5, 4, 3, 2, 1
        ]);
    }

    [Test]
    public async Task ConditionalOrderBy_ConditionFalse_ShouldNotOrder() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalOrderBy(false, orderBy: x => -x).ToList();

        await Assert.That(result).IsEqualTo(_data);
    }

    [Test]
    public async Task ConditionalSkip_ConditionTrue_ShouldSkipElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalSkip(true, 5).ToList();

        await Assert.That(result).IsEqualTo([
            6, 7, 8, 9, 10
        ]);
    }

    [Test]
    public async Task ConditionalSkip_ConditionFalse_ShouldNotSkipElements() {
        IQueryable<int> source = _data.AsQueryable();
        List<int> result = source.ConditionalSkip(false, 5).ToList();

        await Assert.That(result).IsEqualTo(_data);
    }

    [Test]
    public async Task ConditionalDistinct_ConditionTrue_ShouldReturnDistinct() {
        IQueryable<int> source = new List<int> { 1, 2, 2, 3, 3, 3 }.AsQueryable();
        List<int> result = source.ConditionalDistinct(true).ToList();

        await Assert.That(result).IsEqualTo([
            1, 2, 3
        ]);
    }

    [Test]
    public async Task ConditionalDistinct_ConditionFalse_ShouldReturnOriginal() {
        IQueryable<int> source = new List<int> { 1, 2, 2, 3, 3, 3 }.AsQueryable();
        List<int> result = source.ConditionalDistinct(false).ToList();

        await Assert.That(result).IsEqualTo([
            1, 2, 2, 3, 3, 3
        ]);
    }

    [Test]
    public async Task ConditionalUnion_ConditionTrue_ShouldUnion() {
        IQueryable<int> source = new List<int> { 1, 2, 3 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalUnion(true, second).ToList();

        await Assert.That(result).IsEqualTo([
            1, 2, 3, 4, 5
        ]);
    }

    [Test]
    public async Task ConditionalUnion_ConditionFalse_ShouldNotUnion() {
        IQueryable<int> source = new List<int> { 1, 2, 3 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalUnion(false, second).ToList();

        await Assert.That(result).IsEqualTo([
            1, 2, 3
        ]);
    }

    [Test]
    public async Task ConditionalExcept_ConditionTrue_ShouldApplyExcept() {
        IQueryable<int> source = new List<int> { 1, 2, 3, 4 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalExcept(true, second).ToList();

        await Assert.That(result).IsEqualTo([
            1, 2
        ]);
    }

    [Test]
    public async Task ConditionalExcept_ConditionFalse_ShouldNotApplyExcept() {
        IQueryable<int> source = new List<int> { 1, 2, 3, 4 }.AsQueryable();
        IQueryable<int> second = new List<int> { 3, 4, 5 }.AsQueryable();
        List<int> result = source.ConditionalExcept(false, second).ToList();

        await Assert.That(result).IsEqualTo([
            1, 2, 3, 4
        ]);
    }
}
