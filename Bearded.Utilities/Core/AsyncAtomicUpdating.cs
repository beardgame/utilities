namespace Bearded.Utilities
{
    public class AsyncAtomicUpdating<T>
        where T : struct
    {
        private readonly object mutex = new object();

        public T Current { get; private set; }
        public T Previous { get; private set; }
        private T lastRecorded;

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

        public void UpdateToDefault() => UpdateTo(default);

        public void UpdateTo(T state)
        {
            Previous = Current;
            Current = state;
        }
    }
}
