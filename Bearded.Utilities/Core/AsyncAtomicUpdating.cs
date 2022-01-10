namespace Bearded.Utilities;

public sealed class AsyncAtomicUpdating<T>
{
    private readonly object mutex = new object();

    public T Current { get; private set; }
    public T Previous { get; private set; }
    private T lastRecorded;

    public AsyncAtomicUpdating(T initialState)
    {
        Current = initialState;
        Previous = initialState;
        lastRecorded = initialState;
    }

    public void SetLastKnownState(T state)
    {
        lock (mutex)
        {
            lastRecorded = state;
        }
    }

    public void Update()
    {
        lock (mutex)
        {
            UpdateTo(lastRecorded);
        }
    }

    public void UpdateTo(T state)
    {
        Previous = Current;
        Current = state;
    }
}
