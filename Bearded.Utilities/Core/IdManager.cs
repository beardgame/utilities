using System;
using System.Collections.Generic;

namespace Bearded.Utilities;

public sealed class IdManager
{
    private readonly Dictionary<Type, int> lastIds = new();

    public Id<T> GetNext<T>()
    {
        var type = typeof(T);
        var id = lastIds.TryGetValue(type, out var lastId) ? lastId + 1 : 1;
        lastIds[type] = id;
        return new Id<T>(id);
    }
}
