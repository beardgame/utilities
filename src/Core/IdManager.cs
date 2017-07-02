using System;
using System.Collections.Generic;
using Bearded.Utilities.Linq;

namespace Bearded.Utilities
{
    public sealed class IdManager
    {
        private readonly Dictionary<Type, int> lastIds = new Dictionary<Type, int>();

        public Id<T> GetNext<T>()
        {
            var type = typeof(T);
            var id = lastIds.ValueOrDefault(type) + 1;
            lastIds[type] = id;
            return new Id<T>(id);
        }
    }
}
