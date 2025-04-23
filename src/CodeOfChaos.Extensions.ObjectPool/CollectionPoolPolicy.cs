// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Extensions.ObjectPool;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectionPoolPolicy<T, TItem> : IPooledObjectPolicy<T> where T : ICollection<TItem>, new() {
    public T Create() {
        return new T();
    }
    public bool Return(T obj) {
        obj.Clear();
        return obj.Count == 0;
    }
}
