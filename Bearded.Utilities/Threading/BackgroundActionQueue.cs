using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Bearded.Utilities.Threading
{
    /// <summary>
    /// A threadsafe queue that runs scheduled actions on a separate thread.
    /// Typical usage is to schedule actions from one or multiple threads to have them executed in the background.
    /// The actions are guaranteed to be executed in the order they are scheduled.
    /// </summary>
    public sealed class BackgroundActionQueue : IActionQueue
    {
        #region Fields and Properties

        private readonly BlockingCollection<Action> actions = new BlockingCollection<Action>();
        private readonly Thread thread;

        private bool finishing;

        /// <summary>
        /// Gets a value indicating whether this queue has finished or was aborted.
        /// </summary>
        public bool Finished { get; private set; }

        #endregion

        #region Constructor

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

        #endregion

        #region Methods

        private void run()
        {
            while (!finishing)
            {
                actions.Take()();
            }
            Finished = true;
        }

        /// <summary>
        /// Stops the execution of further actions. Actions already queued will still be run.
        /// </summary>
        public void Finish()
        {
            Finish(true);
        }

        /// <summary>
        /// Stops the execution of further actions.
        /// </summary>
        /// <param name="executeScheduled">If true, finishes executing all actions currently scheduled, otherwise stops after the next action.</param>
        public void Finish(bool executeScheduled)
        {
            if (!executeScheduled)
                finishing = true;
            actions.Add(() => finishing = true);
        }

        /// <summary>
        /// Stops this queue from running further action, and aborts any actions currently run.
        /// Since it kills the underlying thread, this may not be a safe way to dispose of the queue, and may lead to data corruption.
        /// Consider using Finish() instead.
        /// </summary>
        public void Abort()
        {
            thread.Abort();
            Finished = true;
        }

        #region IActionQueue

        /// <summary>
        /// Queues an action to run. Returns immediately.
        /// </summary>
        /// <param name="action">The action to run.</param>
        public void RunAndForget(Action action)
        {
            actions.Add(action);
        }

        /// <summary>
        /// Queues an action to run. Returns only after the action has been executed.
        /// </summary>
        /// <param name="action">The action to run.</param>
        public void RunAndAwait(Action action)
        {
            var reset = new ManualResetEvent(false);

            actions.Add(() =>
            {
                action();
                reset.Set();
            });

            reset.WaitOne();
        }

        /// <summary>
        /// Queues a parameterless function to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        public T RunAndReturn<T>(Func<T> action)
        {
            var ret = default(T);
            RunAndAwait(() => ret = action());
            return ret!;
        }

        /// <summary>
        /// Queues a function with one parameter to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The argument for calling the function.</param>
        public T RunAndReturn<TP0, T>(Func<TP0, T> action, TP0 p0)
        {
            return RunAndReturn(() => action(p0));
        }

        /// <summary>
        /// Queues a function with two parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        public T RunAndReturn<TP0, TP1, T>(Func<TP0, TP1, T> action, TP0 p0, TP1 p1)
        {
            return RunAndReturn(() => action(p0, p1));
        }

        /// <summary>
        /// Queues a function with three parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        /// <param name="p2">The third argument for calling the function.</param>
        public T RunAndReturn<TP0, TP1, TP2, T>(Func<TP0, TP1, TP2, T> action, TP0 p0, TP1 p1, TP2 p2)
        {
            return RunAndReturn(() => action(p0, p1, p2));
        }

        /// <summary>
        /// Queues a function with four parameters to run. Returns the return value of the function only after the function has been executed.
        /// </summary>
        /// <param name="action">The function to run.</param>
        /// <param name="p0">The first argument for calling the function.</param>
        /// <param name="p1">The second argument for calling the function.</param>
        /// <param name="p2">The third argument for calling the function.</param>
        /// <param name="p3">The fourth argument for calling the function.</param>
        public T RunAndReturn<TP0, TP1, TP2, TP3, T>(Func<TP0, TP1, TP2, TP3, T> action, TP0 p0, TP1 p1, TP2 p2, TP3 p3)
        {
            return RunAndReturn(() => action(p0, p1, p2, p3));
        }

        #endregion

        #endregion
    }
}
