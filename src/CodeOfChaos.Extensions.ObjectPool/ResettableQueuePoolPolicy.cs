// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Extensions.ObjectPool;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ResettableQueuePoolPolicy<T, TItem> : IPooledObjectPolicy<T> 
    where T : Queue<TItem>, new()
    where TItem : IResettable 
{
    public T Create() => new();
    public bool Return(T obj) {
        while (obj.TryDequeue(out TItem? item)) {
            item.TryReset();
        }
        obj.Clear();
        return obj.Count == 0;
    }
}
