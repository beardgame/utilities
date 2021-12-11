using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bearded.Utilities.IO;

public delegate void LogEvent(Logger.Entry entry);

/// <summary>
/// Automatic class to improve logging by keeping track of logged history, added events, and adding semantics to log messages.
/// Unless otherwise mentioned, the members of this class are thread-safe.
/// </summary>
public class Logger
{
    #region Writers
    public sealed class Writer
    {
        private readonly Logger logger;
        private readonly Severity severity;

        internal Writer(Logger logger, Severity severity)
        {
            this.logger = logger;
            this.severity = severity;
        }

        public void Log(string text) {
            logger.AddEntry(new Entry(text, severity));
        }

        public void Log<T>(T obj) {
            logger.AddEntry(new Entry(obj?.ToString() ?? "null", severity));
        }

        public void Log(string text, params object[] parameters) {
            Log(string.Format(text, parameters));
        }
    }

    public Writer? Fatal { get; }
    public Writer? Error { get; }
    public Writer? Warning { get; }
    public Writer? Info { get; }
    public Writer? Debug { get; }
    public Writer? Trace { get; }
    #endregion

    #region types

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

    public readonly struct Entry : IEquatable<Entry>
    {
        public string Text { get; }

        public Severity Severity { get; }

        public DateTime Time { get; }

        public Entry(string text)
            : this(text, Severity.Debug, DateTime.Now)
        {
        }

        public Entry(string text, Severity severity)
            : this(text, severity, DateTime.Now)
        {
        }

        public Entry(string text, Severity severity, DateTime time)
        {
            Text = text;
            Severity = severity;
            Time = time;
        }

        internal void WriteToConsole()
        {
            ConsoleColor rgb;

            switch (Severity)
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

            Console.ForegroundColor = rgb;
            Console.WriteLine(Text);
            Console.ResetColor();
        }

        public bool Equals(Entry other)
            => string.Equals(Text, other.Text) && Severity == other.Severity && Time.Equals(other.Time);

        public override bool Equals(object? obj) => obj is Entry entry && Equals(entry);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Text?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (int)Severity;
                hashCode = (hashCode * 397) ^ Time.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Entry left, Entry right)
            => left.Equals(right);

        public static bool operator !=(Entry left, Entry right)
            => !left.Equals(right);
    }
    #endregion

    #region fields and constructor

    private readonly List<Entry> lines = new List<Entry>();

    /// <summary>
    /// Gets the stored history of events.
    /// Iterating this collection might throw exceptions if new entries are locked in the mean time, wether by the calling or by other threads.
    /// For thread-safe iteration, use GetSafeRecentEntries.
    /// </summary>
    public ReadOnlyCollection<Entry> RecentEntries { get; }

    /// <summary>
    /// Gets a copy of the recent entry history.
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
    /// Gets a copy of the recent entry history with a minimum severity.
    /// This method allocates significant memory and should not be used unless necessary.
    /// </summary>
    public ReadOnlyCollection<Entry> GetSafeRecentEntriesWithSeverity(Severity severity)
    {
        List<Entry> copy;
        lock (lines)
        {
            copy = lines.Where(entry => entry.Severity <= severity).ToList();
        }
        return copy.AsReadOnly();
    }

    /// <summary>
    /// Adds all recent entries to a provided collection.
    /// </summary>
    public void CopyRecentEntries(ICollection<Entry> collection)
    {
        lock (lines)
        {
            if (collection is List<Entry> list)
                ensureListCapacity(list, list.Count + lines.Count);

            foreach (var entry in lines)
                collection.Add(entry);
        }
    }

    /// <summary>
    /// Adds all recent entries that are at least of the given severity to a provided collection.
    /// </summary>
    public void CopyRecentEntriesWithSeverity(Severity severity, ICollection<Entry> collection)
    {
        lock (lines)
        {
            if (severity == Severity.Trace && collection is List<Entry> list)
                ensureListCapacity(list, list.Count + lines.Count);

            foreach (var entry in lines.Where(entry => entry.Severity <= severity))
                collection.Add(entry);
        }
    }

    private static void ensureListCapacity(List<Entry> list, int neededCapacity)
    {
        if (list.Capacity < neededCapacity)
            list.Capacity = Math.Max(list.Capacity * 2, neededCapacity);
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
    public bool MirrorToConsole { get; set; } = true;

    /// <summary>
    /// If this is true, the Logged event is raised for every added entry.
    /// Default is true.
    /// </summary>
    public bool RaiseEvents { get; set; } = true;

    /// <summary>
    /// If this is true, the logged history will be pruned regularly as controlled by MaxHistoryLength and PrunedLength.
    /// Default is true.
    /// </summary>
    public bool AllowPruning { get; set; } = true;

    /// <summary>
    /// If AllowPruning is true, when the log reaches this length, it is pruned by deleting older entries until it is PrunedLength long.
    /// Default is 10,000.
    /// </summary>
    public int MaxHistoryLength { get; set; } = 10000;

    /// <summary>
    /// This is the length the list is pruned to everytime it reaches MaxHistoryLength.
    /// Default is 5,000.
    /// </summary>
    public int PrunedLength { get; set; } = 5000;

    public Logger() : this(new HashSet<Severity>(Enum.GetValues(typeof(Severity)).Cast<Severity>())) { }

    public Logger(ICollection<Severity> enabledSeverities)
    {
        Fatal = createWriterIfSeverityEnabled(enabledSeverities, Severity.Fatal);
        Error = createWriterIfSeverityEnabled(enabledSeverities, Severity.Error);
        Warning = createWriterIfSeverityEnabled(enabledSeverities, Severity.Warning);
        Info = createWriterIfSeverityEnabled(enabledSeverities, Severity.Info);
        Debug = createWriterIfSeverityEnabled(enabledSeverities, Severity.Debug);
        Trace = createWriterIfSeverityEnabled(enabledSeverities, Severity.Trace);

        RecentEntries = lines.AsReadOnly();
    }

    private Writer? createWriterIfSeverityEnabled(ICollection<Severity> enabledSeverities, Severity severity)
        => enabledSeverities.Contains(severity) ? new Writer(this, severity) : null;

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

            pruneIfNecessary();
        }
    }

    private void pruneIfNecessary()
    {
        if (!AllowPruning)
            return;

        if (lines.Count <= MaxHistoryLength)
            return;

        var toRemove = (lines.Count - PrunedLength).Clamped(0, lines.Count);
        lines.RemoveRange(0, toRemove);
    }

    public void AddEntry(Entry entry)
    {
        addEntrySafe(entry);

        // event is raised here, since if it was called inside lock, it could result in deadlock
        if (RaiseEvents)
            Logged?.Invoke(entry);
    }

    #endregion
}
