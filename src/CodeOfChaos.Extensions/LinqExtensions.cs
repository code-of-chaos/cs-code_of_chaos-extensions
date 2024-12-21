// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LinqExtensions {
    
    public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate) 
        => condition
            ? source.Where(predicate)
            : source;
    
    public static IQueryable<T> ConditionalTake<T>(this IQueryable<T> source, bool condition, int count) 
        => condition && count > 0
            ? source.Take(count)
            : source;
    
    public static IQueryable<T> ConditionalTake<T>(this IQueryable<T> source, bool condition, Range range) 
        => condition
            ? source.Take(range)
            : source;
    
    public static IQueryable<T> ConditionalOrderBy<T>(this IQueryable<T> source, bool condition, Expression<Func<T, object>> orderBy) 
        => condition
            ? source.OrderBy(orderBy)
            : source;
    
    public static IQueryable<T> ConditionalSkip<T>(this IQueryable<T> source, bool condition, int count) 
        => condition && count > 0
            ? source.Skip(count)
            : source;
    
    public static IQueryable<TSource> ConditionalOrderBy<TSource, TKey>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TKey>> orderBy, IComparer<TKey>? comparer) 
        => condition
            ? source.OrderBy(orderBy, comparer)
            : source;
    
    public static IQueryable<TSource> ConditionalOrderByNotNull<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>>? orderBy) 
        => orderBy is not null
            ? source.OrderBy(orderBy)
            : source;
    
    public static IQueryable<TSource> ConditionalOrderByNotNull<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>>? orderBy, IComparer<TKey>? comparer) 
        => orderBy is not null
            ? source.OrderBy(orderBy,comparer)
            : source;
    
    public static IQueryable<TSource> ConditionalQuery<TSource>(this IQueryable<TSource> source, bool condition, Func<IQueryable<TSource>, IQueryable<TSource>> queryableFunc) 
        => condition
            ? queryableFunc(source)
            : source;
    
    public static IQueryable<T> ConditionalDistinct<T>(this IQueryable<T> source, bool condition) 
        => condition
            ? source.Distinct()
            : source;
    
    public static IQueryable<T> ConditionalUnion<T>(this IQueryable<T> source, bool condition, IQueryable<T> second) 
        => condition
            ? source.Union(second)
            : source;
    
    public static IQueryable<T> ConditionalIntersect<T>(this IQueryable<T> source, bool condition, IQueryable<T> second) 
        => condition
            ? source.Intersect(second)
            : source;
    
    public static IQueryable<T> ConditionalExcept<T>(this IQueryable<T> source, bool condition, IQueryable<T> second) 
        => condition
            ? source.Except(second)
            : source;
}
