// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LinqExtensions {
    
    public static IQueryable<T> ConditionalInclude<T>(this IQueryable<T> source, bool condition, Expression<Func<T, object>> include) where T : class 
        => condition
            ? source.Include(include)
            : source;
    
    public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate) =>
        condition
            ? source.Where(predicate)
            : source;
    
    public static IQueryable<T> ConditionalTake<T>(this IQueryable<T> source, bool condition, int count) =>
        condition
            ? source.Take(count)
            : source;
    
    public static IQueryable<T> ConditionalTake<T>(this IQueryable<T> source, bool condition, Range range) =>
        condition
            ? source.Take(range)
            : source;
    
    public static IQueryable<T> ConditionalOrderBy<T>(this IQueryable<T> source, bool condition, Expression<Func<T, object>> orderBy) =>
        condition
            ? source.OrderBy(orderBy)
            : source;
    
    public static IQueryable<TSource> ConditionalOrderBy<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> orderBy, IComparer<TKey>? comparer) =>
        condition
            ? source.OrderBy(orderBy, comparer)
            : source;
    
    public static IQueryable<TSource> ConditionalOrderByNotNull<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>>? orderBy) =>
        orderBy is not null
            ? source.OrderBy(orderBy)
            : source;
}
