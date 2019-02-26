using System;

namespace Bearded.Utilities.Threading
{
    /// <summary>
    /// Interface for queuing actions to run.
    /// </summary>
    public interface IActionQueue
    {
        /// <summary>
        /// Queues an action to run. Returns immediately.
        /// </summary>
        /// <param name="action">The action to run.</param>
        void RunAndForget(Action action);

        /// <summary>
        /// Queues an action to run. Returns only after the action has been executed.
        /// </summary>
        /// <param name="action">The action to run.</param>
        void RunAndAwait(Action action);

        /// <summary>
        /// Queues a parameterless function to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        T RunAndReturn<T>(Func<T> action);

        /// <summary>
        /// Queues a function with one parameter to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The argument for calling the function.</param>
        T RunAndReturn<TP0, T>(Func<TP0, T> action, TP0 p0);

        /// <summary>
        /// Queues a function with two parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        T RunAndReturn<TP0, TP1, T>(Func<TP0, TP1, T> action, TP0 p0, TP1 p1);

        /// <summary>
        /// Queues a function with three parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        /// <param name="p2">The third argument for calling the function.</param>
        T RunAndReturn<TP0, TP1, TP2, T>(Func<TP0, TP1, TP2, T> action, TP0 p0, TP1 p1, TP2 p2);

        /// <summary>
        /// Queues a function with four parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        /// <param name="p2">The third argument for calling the function.</param>
        /// <param name="p3">The fourth argument for calling the function.</param>
        T RunAndReturn<TP0, TP1, TP2, TP3, T>(Func<TP0, TP1, TP2, TP3, T> action, TP0 p0, TP1 p1, TP2 p2, TP3 p3);
    }
}
