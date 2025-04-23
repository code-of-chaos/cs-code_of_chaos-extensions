// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace CodeOfChaos.Extensions.ObjectPool;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StringBuilderPoolPolicy : IPooledObjectPolicy<StringBuilder> {
    public StringBuilder Create() => new();
    public bool Return(StringBuilder obj) {
        obj.Clear();
        return obj.Length == 0;
    }
}
