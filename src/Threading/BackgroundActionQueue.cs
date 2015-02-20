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
        /// <param name="name">Name for the thread executing the actions of this queue.</param>
        /// <param name="backgroundThread">Whether the thread should run in the background or not.</param>
        public BackgroundActionQueue(string name = "Unknown thread", bool backgroundThread = false)
        {
            this.thread = new Thread(this.run)
            {
                Name = name,
                IsBackground = backgroundThread
            };
            this.thread.Start();
        }

        #endregion

        #region Methods

        private void run()
        {
            while (!this.finishing)
            {
                this.actions.Take()();
            }
            this.Finished = true;
        }

        /// <summary>
        /// Stops the execution of further actions.
        /// </summary>
        /// <param name="executeScheduled">If true, finishes executing all actions currently scheduled, otherwise stops after the next action.</param>
        public void Finish(bool executeScheduled = true)
        {
            if (!executeScheduled)
                this.finishing = true;
            this.actions.Add(() => this.finishing = true);
        }

        /// <summary>
        /// Stops this queue from running further action, and aborts any actions currently run.
        /// Since it kills the underlying thread, this may not be a safe way to dispose of the queue, and may lead to data corruption.
        /// Consider using Finish() instead.
        /// </summary>
        public void Abort()
        {
            this.thread.Abort();
            this.Finished = true;
        }

        #region IActionQueue

        public void RunAndAwait(Action action)
        {
            var reset = new ManualResetEvent(false);

            this.actions.Add(() =>
            {
                action();
                reset.Set();
            });

            reset.WaitOne();
        }

        public void RunAndForget(Action action)
        {
            this.actions.Add(action);
        }

        public T RunAndReturn<T>(Func<T> action)
        {
            T ret = default(T);
            this.RunAndAwait(() => ret = action());
            return ret;
        }

        public T RunAndReturn<TP0, T>(Func<TP0, T> action, TP0 p0)
        {
            return this.RunAndReturn(() => action(p0));
        }

        public T RunAndReturn<TP0, TP1, T>(Func<TP0, TP1, T> action, TP0 p0, TP1 p1)
        {
            return this.RunAndReturn(() => action(p0, p1));
        }

        public T RunAndReturn<TP0, TP1, TP2, T>(Func<TP0, TP1, TP2, T> action, TP0 p0, TP1 p1, TP2 p2)
        {
            return this.RunAndReturn(() => action(p0, p1, p2));
        }

        public T RunAndReturn<TP0, TP1, TP2, TP3, T>(Func<TP0, TP1, TP2, TP3, T> action, TP0 p0, TP1 p1, TP2 p2, TP3 p3)
        {
            return this.RunAndReturn(() => action(p0, p1, p2, p3));
        }

        #endregion

        #endregion
    }
}
