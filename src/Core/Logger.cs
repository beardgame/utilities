using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bearded.Utilities
{
    /// <summary>
    /// An event containing a log entry.
    /// </summary>
    public delegate void LogEvent(Logger.Entry entry);

    /// <summary>
    /// Automatic class to improve logging by keeping track of logged history, added events, and adding semantics to log messages.
    /// Unless otherwise mentioned, the members of this class are thread-safe.
    /// </summary>
    public class Logger
    {
        #region Writers
        /// <summary>
        /// Provides an interface to log messages at a specific severity.
        /// </summary>
        public sealed class Writer
        {
            private readonly Logger logger;
            private readonly Severity severity;

            internal Writer(Logger logger, Severity severity)
            {
                this.logger = logger;
                this.severity = severity;
            }

            /// <summary>
            /// Logs a message.
            /// </summary>
            /// <param name="text">Text to log.</param>
            public void Log(string text) {
                logger.AddEntry(new Entry(text, this.severity));
            }

            /// <summary>
            /// Logs an object.
            /// </summary>
            /// <param name="obj">Value to log.</param>
            public void Log<T>(T obj) {
                logger.AddEntry(new Entry(obj.ToString(), this.severity));
            }

            /// <summary>
            /// Logs a message.
            /// </summary>
            /// <param name="text">Template string of the error.</param>
            /// <param name="parameters">Parameters inserted into template string.</param>
            public void Log(string text, params object[] parameters) {
                this.Log(string.Format(text, parameters));
            }
        }

        /// <summary>
        /// Writer to log fatal messages.
        /// </summary>
        public Writer Fatal { get; }
        /// <summary>
        /// Writer to log error messages.
        /// </summary>
        public Writer Error { get; }
        /// <summary>
        /// Writer to log warning messages.
        /// </summary>
        public Writer Warning { get; }
        /// <summary>
        /// Writer to log info messages.
        /// </summary>
        public Writer Info { get; }
        /// <summary>
        /// Writer to log debug messages.
        /// </summary>
        public Writer Debug { get; }
        /// <summary>
        /// Writer to log trace messages.
        /// </summary>
        public Writer Trace { get; }
        #endregion

        #region types

        /// <summary>
        /// Used to add semantic categories to log messages.
        /// </summary>
        public enum Severity
        {
            /// <summary>
            /// Used for any error that forces an application shutdown.
            /// </summary>
            Fatal = 0,
            /// <summary>
            /// Used for errors that are fatal to the operation, but leave the application alive.
            /// </summary>
            Error = 1,
            /// <summary>
            /// Used for anything that can cause application oddities.
            /// </summary>
            Warning = 2,
            /// <summary>
            /// Used for generally useful information to log.
            /// </summary>
            Info = 3,
            /// <summary>
            /// Used for information that is diagnostically helpful to people more than just developers.
            /// </summary>
            Debug = 4,
            /// <summary>
            /// Used for tracing purposes only.
            /// </summary>
            Trace = 5,
        }

        /// <summary>
        /// A log entry.
        /// </summary>
        public struct Entry
        {
            private readonly string text;
            private readonly Severity severity;
            private readonly DateTime time;

            /// <summary>
            /// Creates a log entry.
            /// </summary>
            /// <param name="text">The text of the entry.</param>
            /// <param name="severity">The severity of the entry.</param>
            public Entry(string text, Severity severity = Severity.Debug)
                : this(text, severity, DateTime.Now)
            {
            }

            /// <summary>
            /// Creates a log entry that overrides the current time.
            /// </summary>
            /// <param name="text">The text of the entry.</param>
            /// <param name="severity">The severity of the entry.</param>
            /// <param name="time">The time for the log entry.</param>
            public Entry(string text, Severity severity, DateTime time)
            {
                this.text = text;
                this.severity = severity;
                this.time = time;
            }

            /// <summary>
            /// Gets the text of the log entry.
            /// </summary>
            public string Text { get { return this.text; } }

            /// <summary>
            /// Gets the severity of the log entry.
            /// </summary>
            public Severity Severity { get { return this.severity; } }

            /// <summary>
            /// Gets the time of the log entry.
            /// </summary>
            public DateTime Time { get { return this.time; } }

            internal void WriteToConsole()
            {
                var rgb = ConsoleColor.White;

                switch (this.Severity)
                {
                    case Severity.Fatal:
                    case Severity.Error:
                        rgb = ConsoleColor.Red;
                        break;
                    case Severity.Warning:
                        rgb = ConsoleColor.Yellow;
                        break;
                    case Severity.Info:
                        rgb = ConsoleColor.White;
                        break;
                    case Severity.Debug:
                        rgb = ConsoleColor.Green;
                        break;
                    case Severity.Trace:
                        rgb = ConsoleColor.Blue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                System.Console.ForegroundColor = rgb;
                System.Console.WriteLine(this.Text);
                System.Console.ResetColor();
            }
        }
        #endregion

        #region fields and constructor

        private readonly List<Entry> lines = new List<Entry>();
        private readonly ReadOnlyCollection<Entry> linesAsReadonly;
        /// <summary>
        /// Gets the stored history of events.
        /// Iterating this collection might throw exceptions if new entries are locked in the mean time, wether by the calling or by other threads.
        /// For thread-safe iteration, use GetSafeRecentEntries.
        /// </summary>
        public ReadOnlyCollection<Entry> RecentEntries { get { return linesAsReadonly; } }

        /// <summary>
        /// Gets a copy of the recent entry history.
        /// This is the only thread-safe way of accessing the history.
        /// This method allocates significant memory and should not be used unless necessary.
        /// </summary>
        public ReadOnlyCollection<Entry> GetSafeRecentEntries()
        {
            List<Entry> copy;
            lock (lines)
            {
                copy = lines.ToList();
            }
            return copy.AsReadOnly();
        }

        /// <summary>
        /// If RaiseEvents is true, this event is raised every time an event is added to the log.
        /// </summary>
        public event LogEvent Logged;

        #region settings

        /// <summary>
        /// If this is true, all added events are automatically written to the standard output.
        /// Default is true.
        /// </summary>
        public bool MirrorToConsole { get; set; }
        /// <summary>
        /// If this is true, the Logged event is raised for every added entry.
        /// Default is true.
        /// </summary>
        public bool RaiseEvents { get; set; }
        /// <summary>
        /// If this is true, the logged history will be pruned regularly as controlled by MaxHistoryLength and PrunedLength.
        /// Default is true.
        /// </summary>
        public bool AllowPruning { get; set; }
        /// <summary>
        /// If AllowPruning is true, when the log reaches this length, it is pruned by deleting older entries until it is PrunedLength long.
        /// Default is 10,000.
        /// </summary>
        public int MaxHistoryLength { get; set; }
        /// <summary>
        /// This is the length the list is pruned to everytime it reaches MaxHistoryLength.
        /// Default is 5,000.
        /// </summary>
        public int PrunedLength { get; set; }

        internal Logger()
        {
            Fatal = new Writer(this, Severity.Fatal);
            Error = new Writer(this, Severity.Error);
            Warning = new Writer(this, Severity.Warning);
            Info = new Writer(this, Severity.Info);
            Debug = new Writer(this, Severity.Debug);
            Trace = new Writer(this, Severity.Trace);

            linesAsReadonly = lines.AsReadOnly();

            MirrorToConsole = true;
            RaiseEvents = true;
            AllowPruning = true;
            MaxHistoryLength = 10000;
            PrunedLength = 5000;
        }

        #endregion

        #endregion

        #region log methods

        private void addEntrySafe(Entry entry)
        {
            lock (lines)
            {
                lines.Add(entry);

                if (MirrorToConsole)
                    entry.WriteToConsole();

                if (AllowPruning)
                {
                    if (lines.Count > MaxHistoryLength)
                    {
                        var toRemove = System.Math.Min(System.Math.Max(lines.Count - PrunedLength, 0), lines.Count);
                        lines.RemoveRange(0, toRemove);
                    }
                }
            }
        }

        private void invokeEvent(LogEvent e, Entry entry)
        {
            if (e != null)
                e(entry);
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        public void AddEntry(Entry entry)
        {
            addEntrySafe(entry);

            // event is raised here, since if it was called inside lock, it could result in deadlock
            if (RaiseEvents)
                invokeEvent(Logged, entry);
        }

        #endregion
    }
}
