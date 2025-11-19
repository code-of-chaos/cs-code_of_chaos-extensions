// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics;

namespace Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class LinqWithExtensions {

    [DebuggerStepThrough]
    public static IQueryable<T> With<T>(this IQueryable<T> source, Func<IQueryable<T>, IQueryable<T>> query)
        => query(source);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0>(this IQueryable<T> source, Func<IQueryable<T>, T0, IQueryable<T>> query, T0 arg0)
        => query(source, arg0);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0, T1>(this IQueryable<T> source, Func<IQueryable<T>, T0, T1, IQueryable<T>> query, T0 arg0, T1 arg1)
        => query(source, arg0, arg1);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0, T1, T2>(this IQueryable<T> source, Func<IQueryable<T>, T0, T1, T2, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2)
        => query(source, arg0, arg1, arg2);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0, T1, T2, T3>(this IQueryable<T> source, Func<IQueryable<T>, T0, T1, T2, T3, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        => query(source, arg0, arg1, arg2, arg3);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0, T1, T2, T3, T4>(this IQueryable<T> source, Func<IQueryable<T>, T0, T1, T2, T3, T4, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        => query(source, arg0, arg1, arg2, arg3, arg4);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0, T1, T2, T3, T4, T5>(this IQueryable<T> source, Func<IQueryable<T>, T0, T1, T2, T3, T4, T5, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        => query(source, arg0, arg1, arg2, arg3, arg4, arg5);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0, T1, T2, T3, T4, T5, T6>(this IQueryable<T> source, Func<IQueryable<T>, T0, T1, T2, T3, T4, T5, T6, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        => query(source, arg0, arg1, arg2, arg3, arg4, arg5, arg6);

    [DebuggerStepThrough]
    public static IQueryable<T> With<T, T0, T1, T2, T3, T4, T5, T6, T7>(this IQueryable<T> source, Func<IQueryable<T>, T0, T1, T2, T3, T4, T5, T6, T7, IQueryable<T>> query, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        => query(source, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
}
