// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TaskHelper {
    public static Task<T?> FromTaskOrDefault<T>(Task<T?>? originalTask) {
        if (originalTask != null) return originalTask;

        if (typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null) {
            // Non-nullable value types (like int, double)
            return Task.FromResult(default(T));
        }

        // Nullable value types or reference types
        return Task.FromResult<T?>(default);
    }

    public static Task<T?> FromTaskOrDefault<T>(Task<T?>? originalTask, T? defaultValue)
        => originalTask ?? Task.FromResult(defaultValue);

    public static Task<T?> FromTaskOrDefault<T>(Task<T?>? originalTask, Func<T?> defaultValueFactory)
        => originalTask ?? Task.FromResult(defaultValueFactory.Invoke());
}
