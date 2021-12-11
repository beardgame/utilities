using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bearded.Utilities.Threading;

/// <summary>
/// A thread-safe queue to run actions from.
/// Typical usage is to schedule actions from multiple threads and execute them on one main thread.
/// However, actions can also be executed by multiple threads.
/// If only one thread is used to execute, the actions are guaranteed to be executed in the order they were scheduled.
/// </summary>
public sealed class ManualActionQueue : IActionQueue
{
    private readonly BlockingCollection<Action> actions = new BlockingCollection<Action>();

    /// <summary>
    /// Executes one scheduled action.
    /// If no action is scheduled, this will wait until one is scheduled, and then execute that.
    /// This will never return without having executed exactly one scheduled action.
    /// </summary>
    public void ExecuteOne()
    {
        var action = actions.Take();
        action();
    }

    /// <summary>
    /// Executes one scheduled action, if any are available.
    /// Returns immediately if no actions are scheduled.
    /// </summary>
    /// <returns>Whether an action was executed.</returns>
    public bool TryExecuteOne()
    {
        if (actions.TryTake(out var action))
        {
            action();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to execute one scheduled action.
    /// If no action is available immediately, it will wait the given time span for an action to be scheduled before returning.
    /// </summary>
    /// <param name="timeout">The time span.</param>
    /// <returns>Whether an action was executed.</returns>
    public bool TryExecuteOne(TimeSpan timeout)
    {
        if (actions.TryTake(out var action, timeout))
        {
            action();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Executes scheduled actions for a given time span.
    /// Returns after the first action, if the given time has elapsed.
    /// May run longer than the given time, depending on the scheduled actions, but will never take less.
    /// </summary>
    /// <param name="time">The time span.</param>
    /// <returns>The number of actions executed.</returns>
    public int ExecuteFor(TimeSpan time)
    {
        var timer = Stopwatch.StartNew();
        var executed = 0;
        while (true)
        {
            var timeLeft = time - timer.Elapsed;
            if (timeLeft < new TimeSpan(0))
                break;
            if (!actions.TryTake(out var action, timeLeft))
                break;
            executed++;
            action();
        }
        return executed;
    }

    public void Queue(Action action)
    {
        actions.Add(action);
    }

    public Task Run(Action action)
    {
        var task = new TaskCompletionSource<object?>();

        actions.Add(() =>
        {
            action();
            task.SetResult(null);
        });

        return task.Task;
    }

    public Task<T> Run<T>(Func<T> function)
    {
        var task = new TaskCompletionSource<T>();

        actions.Add(() =>
        {
            var result = function();
            task.SetResult(result);
        });

        return task.Task;
    }
}
