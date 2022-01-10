using System.Collections.Generic;

namespace Bearded.Utilities.Tests.Collections;

public static class LoopingQueue
{
    public static LoopingQueue<T> Looping<T>(this IEnumerable<T> items) => new LoopingQueue<T>(items);
}

public class LoopingQueue<T>
{
    private readonly Queue<T> queue;

    public LoopingQueue(IEnumerable<T> items)
    {
        queue = new Queue<T>(items);
    }

    public T Next()
    {
        var item = queue.Dequeue();
        queue.Enqueue(item);
        return item;
    }
}
