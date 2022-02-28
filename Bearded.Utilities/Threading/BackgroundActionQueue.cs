using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bearded.Utilities.Threading;

/// <summary>
/// A thread-safe queue that runs scheduled actions on a separate thread.
/// Typical usage is to schedule actions from one or multiple threads to have them executed in the background.
/// The actions are guaranteed to be executed in the order they are scheduled.
/// </summary>
public sealed class BackgroundActionQueue : IActionQueue
{
    private readonly ManualActionQueue queue = new ManualActionQueue();

    private readonly Thread thread;

    private bool finishing;

    /// <summary>
    /// Gets a value indicating whether this queue has finished or was aborted.
    /// </summary>
    public bool Finished { get; private set; }

    /// <summary>
    /// Creates a new background action queue.
    /// </summary>
    public BackgroundActionQueue()
        : this("Unknown thread", false)
    {
    }

    /// <summary>
    /// Creates a new background action queue.
    /// </summary>
    /// <param name="name">Name for the thread executing the actions of this queue.</param>
    /// <param name="backgroundThread">Whether the thread should run in the background or not.</param>
    public BackgroundActionQueue(string name, bool backgroundThread)
    {
        thread = new Thread(run)
        {
            Name = name,
            IsBackground = backgroundThread
        };
        thread.Start();
    }

    private void run()
    {
        while (!finishing)
        {
            queue.ExecuteOne();
        }
        Finished = true;
    }

    /// <summary>
    /// Stops the execution of further actions. Actions already queued will still be run.
    /// </summary>
    public Task Finish()
    {
        return Finish(true);
    }

    /// <summary>
    /// Stops the execution of further actions.
    /// </summary>
    /// <param name="executeScheduled">If true, finishes executing all actions currently scheduled, otherwise stops after the current action.</param>
    public Task Finish(bool executeScheduled)
    {
        if (!executeScheduled)
            finishing = true;
        return queue.Run(() => finishing = true);
    }

    public void Queue(Action action) => queue.Queue(action);
    public Task Run(Action action) => queue.Run(action);
    public Task<T> Run<T>(Func<T> function) => queue.Run(function);
}
