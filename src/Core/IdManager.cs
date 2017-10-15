using System;
using System.Collections.Generic;

namespace Bearded.Utilities
{
    public sealed class IdManager
    {
        private readonly Dictionary<Type, int> lastIds = new Dictionary<Type, int>();

        public Id<T> GetNext<T>()
        {
            var type = typeof(T);
            int id;
            if (lastIds.TryGetValue(type, out var lastId))
            {
                id = lastId + 1;
            }
            else
            {
                id = 1;
            }
            lastIds[type] = id;
            return new Id<T>(id);
        }
    }
}
