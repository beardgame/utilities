using System;

namespace Bearded.Utilities;

public abstract class Singleton<TSelf> where TSelf : Singleton<TSelf>
{
    public static TSelf Instance { get; private set; } = null!;

    protected Singleton()
    {
        if (Instance != null)
            throw new InvalidOperationException("A singleton can only be instantiated once.");

        Instance = (TSelf)this;
    }
}
