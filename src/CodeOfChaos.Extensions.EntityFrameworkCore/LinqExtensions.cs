// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LinqExtensions {
    
    public static IQueryable<T> ConditionalInclude<T>(this IQueryable<T> source, bool condition, Expression<Func<T, object>> include) where T : class 
        => condition
            ? source.Include(include)
            : source;
    
    
}
