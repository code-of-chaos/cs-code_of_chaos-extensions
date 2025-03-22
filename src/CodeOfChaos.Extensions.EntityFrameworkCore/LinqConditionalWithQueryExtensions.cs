// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics;

namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LinqConditionalWithExtensions {

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> query)
        => condition
            ? query(source)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, IQueryable<T>> query, T0 arg0)
        => condition
            ? query(source, arg0)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0, T1>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, T1, IQueryable<T>> query, T0 arg0, T1 arg1)
        => condition
            ? query(source, arg0, arg1)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0, T1, T2>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, T1, T2, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2)
        => condition
            ? query(source, arg0, arg1, arg2)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0, T1, T2, T3>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, T1, T2, T3, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        => condition
            ? query(source, arg0, arg1, arg2, arg3)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0, T1, T2, T3, T4>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, T1, T2, T3, T4, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        => condition
            ? query(source, arg0, arg1, arg2, arg3, arg4)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0, T1, T2, T3, T4, T5>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, T1, T2, T3, T4, T5, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        => condition
            ? query(source, arg0, arg1, arg2, arg3, arg4, arg5)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0, T1, T2, T3, T4, T5, T6>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, T1, T2, T3, T4, T5, T6, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        => condition
            ? query(source, arg0, arg1, arg2, arg3, arg4, arg5, arg6)
            : source;

    [DebuggerStepThrough]
    public static IQueryable<T> ConditionalWith<T, T0, T1, T2, T3, T4, T5, T6, T7>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, T0, T1, T2, T3, T4, T5, T6, T7, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        => condition
            ? query(source, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7)
            : source;
}
