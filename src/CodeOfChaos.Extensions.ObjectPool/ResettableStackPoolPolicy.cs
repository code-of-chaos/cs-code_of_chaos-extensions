// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Extensions.ObjectPool;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ResettableStackPoolPolicy<T, TItem> : IPooledObjectPolicy<T> 
    where T : Stack<TItem>, new()
    where TItem : IResettable 
{
    public T Create() => new();
    public bool Return(T obj) {
        while (obj.TryPop(out TItem? item)) {
            item.TryReset();
        }
        obj.Clear();
        return obj.Count == 0;
    }
}
