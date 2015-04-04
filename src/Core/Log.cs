using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bearded.Utilities
{
    /// <summary>
    /// An event containing a log entry.
    /// </summary>
    public delegate void LogEvent(Log.Entry entry);

    /// <summary>
    /// Automatic class to improve logging by keeping track of logged history, added events, and adding semantics to log messages.
    /// Unless otherwise mentioned, the members of this class are thread-safe.
    /// </summary>
    public static class Log
    {
        #region types

        /// <summary>
        /// Used to add semantic categories to log messages.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// A regular message.
            /// </summary>
            Message = 0,
            /// <summary>
            /// A warning, used for unintended or unexpected behaviour.
            /// </summary>
            Warning,
            /// <summary>
            /// An error more severe than a warning.
            /// </summary>
            Error,
            /// <summary>
            /// A debug message. Entries with this type are only added to the log in DEBUG mode.
            /// </summary>
            Debug,
            /// <summary>
            /// Special type for user terminal feedback, or other highlighting.
            /// Usage is application dependent.
            /// </summary>
            Special,
        }

        /// <summary>
        /// A log entry.
        /// </summary>
        public struct Entry
        {
            private readonly string text;
            private readonly Type type;
            private readonly DateTime time;

            /// <summary>
            /// Creates a log entry.
            /// </summary>
            /// <param name="text">The text of the entry.</param>
            /// <param name="type">The type of the entry.</param>
            public Entry(string text, Type type = Type.Message)
                : this(text, type, DateTime.Now)
            {
            }

            /// <summary>
            /// Creates a log entry that overrides the current time.
            /// </summary>
            /// <param name="text">The text of the entry.</param>
            /// <param name="type">The type of the entry.</param>
            /// <param name="time">The time for the log entry.</param>
            public Entry(string text, Type type, DateTime time)
            {
                this.text = text;
                this.type = type;
                this.time = time;
            }

            /// <summary>
            /// Gets the text of the log entry.
            /// </summary>
            public string Text { get { return this.text; } }

            /// <summary>
            /// Gets the type of the log entry.
            /// </summary>
            public Type Type { get { return this.type; } }

            /// <summary>
            /// Gets the time of the log entry.
            /// </summary>
            public DateTime Time { get { return this.time; } }

            internal void WriteToConsole()
            {
                var rgb = ConsoleColor.White;

                switch (this.Type)
                {
                    case Type.Message:
                        break;
                    case Type.Warning:
                        rgb = ConsoleColor.Yellow;
                        break;
                    case Type.Error:
                        rgb = ConsoleColor.Red;
                        break;
                    case Type.Debug:
                        rgb = ConsoleColor.Green;
                        break;
                    case Type.Special:
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

        private static readonly List<Entry> lines = new List<Entry>();
        private static readonly ReadOnlyCollection<Entry> linesAsReadonly = lines.AsReadOnly();
        /// <summary>
        /// Gets the stored history of events.
        /// Iterating this collection might throw exceptions if new entries are locked in the mean time, wether by the calling or by other threads.
        /// For thread-safe iteration, use GetSafeRecentEntries.
        /// </summary>
        public static ReadOnlyCollection<Entry> RecentEntries { get { return linesAsReadonly; } }

        /// <summary>
        /// Gets a copy of the recent entry history.
        /// This is the only thread-safe way of accessing the history.
        /// This method allocates significant memory and should not be used unless necessary.
        /// </summary>
        public static ReadOnlyCollection<Entry> GetSafeRecentEntries()
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
        public static event LogEvent Logged;

        #region settings

        /// <summary>
        /// If this is true, all added events are automatically written to the standard output.
        /// Default is true.
        /// </summary>
        public static bool MirrorToConsole { get; set; }
        /// <summary>
        /// If this is true, the Logged event is raised for every added entry.
        /// Default is true.
        /// </summary>
        public static bool RaiseEvents { get; set; }
        /// <summary>
        /// If this is true, the logged history will be pruned regularly as controlled by MaxHistoryLength and PrunedLength.
        /// Default is true.
        /// </summary>
        public static bool AllowPruning { get; set; }
        /// <summary>
        /// If AllowPruning is true, when the log reaches this length, it is pruned by deleting older entries until it is PrunedLength long.
        /// Default is 10,000.
        /// </summary>
        public static int MaxHistoryLength { get; set; }
        /// <summary>
        /// This is the length the list is pruned to everytime it reaches MaxHistoryLength.
        /// Default is 5,000.
        /// </summary>
        public static int PrunedLength { get; set; }

        static Log()
        {
            MirrorToConsole = true;
            RaiseEvents = true;
            AllowPruning = true;
            MaxHistoryLength = 10000;
            PrunedLength = 5000;
        }

        #endregion

        #endregion

        #region methods

        #region log methods

        private static void addEntrySafe(Entry entry)
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

        private static void invokeEvent(LogEvent e, Entry entry)
        {
            if (e != null)
                e(entry);
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        public static void AddEntry(Entry entry)
        {
            addEntrySafe(entry);

            // event is raised here, since if it was called inside lock, it could result in deadlock
            if (RaiseEvents)
                invokeEvent(Logged, entry);
        }

        #region Line()
        /// <summary>
        /// Adds a regular message to the log.
        /// </summary>
        /// <param name="text">Text of the message.</param>
        public static void Line(string text)
        {
            // Consistency is cake.
            AddEntry(new Entry(text));
        }

        /// <summary>
        /// Adds a regular message to the log.
        /// </summary>
        /// <param name="obj">Value to log in the message.</param>
        public static void Line<T>(T obj)
        {
            AddEntry(new Entry(obj.ToString()));
        }

        /// <summary>
        /// Adds a regular message to the log.
        /// </summary>
        /// <param name="text">Template string of the message.</param>
        /// <param name="parameters">Parameters inserted into template string.</param>
        public static void Line(string text, params object[] parameters)
        {
            Line(string.Format(text, parameters));
        }
        #endregion

        #region Warning()
        /// <summary>
        /// Adds a warning to the log.
        /// </summary>
        /// <param name="text">Text of the warning.</param>
        public static void Warning(string text)
        {
            AddEntry(new Entry(text, Type.Warning));
        }

        /// <summary>
        /// Adds a warning to the log.
        /// </summary>
        /// <param name="obj">Value to log in the warning.</param>
        public static void Warning<T>(T obj)
        {
            AddEntry(new Entry(obj.ToString(), Type.Warning));
        }

        /// <summary>
        /// Adds a warning to the log.
        /// </summary>
        /// <param name="text">Template string of the warning.</param>
        /// <param name="parameters">Parameters inserted into template string.</param>
        public static void Warning(string text, params object[] parameters)
        {
            Warning(string.Format(text, parameters));
        }
        #endregion

        #region Error()
        /// <summary>
        /// Adds an error to the log.
        /// </summary>
        /// <param name="text">Text of the error.</param>
        public static void Error(string text)
        {
            AddEntry(new Entry(text, Type.Error));
        }

        /// <summary>
        /// Adds an error to the log.
        /// </summary>
        /// <param name="obj">Value to log in the error.</param>
        public static void Error<T>(T obj)
        {
            AddEntry(new Entry(obj.ToString(), Type.Error));
        }

        /// <summary>
        /// Adds an error to the log.
        /// </summary>
        /// <param name="text">Template string of the error.</param>
        /// <param name="parameters">Parameters inserted into template string.</param>
        public static void Error(string text, params object[] parameters)
        {
            Error(string.Format(text, parameters));
        }
        #endregion

        #region Debug()
        /// <summary>
        /// Adds a debug message to the log.
        /// If the application is not run in DEBUG mode, this has no effect.
        /// </summary>
        /// <param name="text">Text of the message.</param>
        public static void Debug(string text)
        {
#if !DEBUG
            return;
#endif
            AddEntry(new Entry(text, Type.Debug));
        }

        /// <summary>
        /// Adds a debug message to the log.
        /// </summary>
        /// <param name="obj">Value to log in the debug message.</param>
        public static void Debug<T>(T obj)
        {
#if !DEBUG
            return;
#endif
            AddEntry(new Entry(obj.ToString(), Type.Debug));
        }

        /// <summary>
        /// Adds a debug message to the log.
        /// If the application is not run in DEBUG mode, this has no effect.
        /// </summary>
        /// <param name="text">Template string of the message.</param>
        /// <param name="parameters">Parameters inserted into template string.</param>
        public static void Debug(string text, params object[] parameters)
        {
#if !DEBUG
            return;
#endif
            Debug(string.Format(text, parameters));
        }
        #endregion

        #region Special()
        /// <summary>
        /// Adds a special message to the log.
        /// </summary>
        /// <param name="text">Text of the message.</param>
        public static void Special(string text)
        {
            AddEntry(new Entry(text, Type.Special));
        }

        /// <summary>
        /// Adds a special message to the log.
        /// </summary>
        /// <param name="obj">Value to log in the special message.</param>
        public static void Special<T>(T obj)
        {
            AddEntry(new Entry(obj.ToString(), Type.Special));
        }

        /// <summary>
        /// Adds a special message to the log.
        /// </summary>
        /// <param name="text">Template string of the message.</param>
        /// <param name="parameters">Parameters inserted into template string.</param>
        public static void Special(string text, params object[] parameters)
        {
            Special(string.Format(text, parameters));
        }
        #endregion

        #endregion

        #endregion
    }
}
