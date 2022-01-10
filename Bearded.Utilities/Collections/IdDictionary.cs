using System.Collections.Generic;

namespace Bearded.Utilities.Collections;

public class IdDictionary<T> : Dictionary<Id<T>, T>
    where T : IIdable<T>
{
    public void Add(T item) => Add(item.Id, item);

    public void Remove(T item) => Remove(item.Id);
}
