// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TaskWhenAllHelper {
    
    // ValueTask because in theory tasks can be null and thus we quite without the need for a task
    private static async ValueTask WhenAllWrapper(params Task[] tasks) {
        int taskCount = tasks.Length;

        // Do some checks before we run the tasks
        for (int i = 0; i < taskCount; i++) {
            Task task = tasks[i];
            ArgumentNullException.ThrowIfNull(task);
        }

        await Task.WhenAll(tasks);

        // Check if the tasks have successfully run
        var exceptionsToThrow = new List<Exception>();
        for (int i = 0; i < taskCount; i++) {
            switch (tasks[i]) {
                case { IsCanceled: true } task: {
                    exceptionsToThrow.Add(new TaskCanceledException(task));
                    break;
                }

                case { IsFaulted: true } task: {
                    exceptionsToThrow.Add(task.Exception);
                    break;
                }
            }
        }

        if (exceptionsToThrow.Count != 0) throw new AggregateException(exceptionsToThrow);
    }

    public static async Task<(T0, T1)> WhenAll<T0, T1>(Task<T0> task0, Task<T1> task1) {
        await WhenAllWrapper(task0, task1);
        return (task0.Result, task1.Result);
    }

    public static async Task<(T0, T1, T2)> WhenAll<T0, T1, T2>(Task<T0> task0, Task<T1> task1, Task<T2> task2) {
        await WhenAllWrapper(task0, task1, task2);
        return (task0.Result, task1.Result, task2.Result);
    }

    public static async Task<(T0, T1, T2, T3)> WhenAll<T0, T1, T2, T3>(Task<T0> task0, Task<T1> task1, Task<T2> task2, Task<T3> task3) {
        await WhenAllWrapper(task0, task1, task2, task3);
        return (task0.Result, task1.Result, task2.Result, task3.Result);
    }

    public static async Task<(T0, T1, T2, T3, T4)> WhenAll<T0, T1, T2, T3, T4>(Task<T0> task0, Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4) {
        await WhenAllWrapper(task0, task1, task2, task3, task4);
        return (task0.Result, task1.Result, task2.Result, task3.Result, task4.Result);
    }

    public static async Task<(T0, T1, T2, T3, T4, T5)> WhenAll<T0, T1, T2, T3, T4, T5>(Task<T0> task0, Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5) {
        await WhenAllWrapper(task0, task1, task2, task3, task4, task5);
        return (task0.Result, task1.Result, task2.Result, task3.Result, task4.Result, task5.Result);
    }

    public static async Task<(T0, T1, T2, T3, T4, T5, T6)> WhenAll<T0, T1, T2, T3, T4, T5, T6>(Task<T0> task0, Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6) {
        await WhenAllWrapper(task0, task1, task2, task3, task4, task5, task6);
        return (task0.Result, task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result);
    }

    public static async Task<(T0, T1, T2, T3, T4, T5, T6, T7)> WhenAll<T0, T1, T2, T3, T4, T5, T6, T7>(Task<T0> task0, Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7) {
        await WhenAllWrapper(task0, task1, task2, task3, task4, task5, task6, task7);
        return (task0.Result, task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result);
    }
}
