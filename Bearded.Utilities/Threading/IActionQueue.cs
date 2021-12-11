using System;
using System.Threading.Tasks;

namespace Bearded.Utilities.Threading;

/// <summary>
/// Interface for queuing actions to run.
/// </summary>
public interface IActionQueue
{
    /// <summary>
    /// Queues an action to run. Returns immediately.
    /// </summary>
    /// <param name="action">The action to run.</param>
    void Queue(Action action);

    /// <summary>
    /// Queues an action to run. Returns a task that completes when the action has been executed.
    /// </summary>
    /// <param name="action">The action to run.</param>
    Task Run(Action action);

    /// <summary>
    /// Queues a parameterless function to run. Returns a task that completes when the function has been executed.
    /// </summary>
    /// <param name="function">The function to run.</param>
    Task<T> Run<T>(Func<T> function);
}
