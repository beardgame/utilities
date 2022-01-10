using System;
using System.IO;

namespace Bearded.Utilities.IO;

/// <summary>
/// This class can be used to watch a file for modifications by checking its last write time stamp.
/// </summary>
public class FileModifiedWatcher
{
    private DateTime? lastModified;

    /// <summary>
    /// Gets the path of the file watched.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the filename, without directories, of the file watched.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Creates a new <see cref="FileModifiedWatcher"/> watching the specified file.
    /// </summary>
    /// <param name="path">The file to watch. Must be valid path.
    /// Otherwise, behaviour is undefined and exceptions may be thrown.</param>
    public FileModifiedWatcher(string path)
    {
        Path = path;
        FileName = System.IO.Path.GetFileName(path);

        Reset();
    }

    private DateTime? getLastWriteTime()
    {
        return File.Exists(Path)
            ? (DateTime?)File.GetLastWriteTime(Path)
            : null;
    }

    /// <summary>
    /// Resets this watcher to ignore all past modifications to file.
    /// </summary>
    public void Reset()
    {
        lastModified = getLastWriteTime();
    }

    /// <summary>
    /// Checks whether the file was changed since the last reset of the watcher, and then resets the watcher.
    /// </summary>
    /// <returns>True, if the file has a different last-write time stamp than when the watcher was reset last.
    /// True if the file was created or deleted since the last reset.
    /// False otherwise.</returns>
    public bool WasModified() => WasModified(true);

    /// <summary>
    /// Checks whether the file was changed since the last reset of the watcher.
    /// </summary>
    /// <param name="resetModified">Whether to reset the watcher after checking for changes.</param>
    /// <returns>True, if the file has a different last-write time stamp than when the watcher was reset last.
    /// True if the file was created or deleted since the last reset.
    /// False otherwise.</returns>
    public bool WasModified(bool resetModified)
    {
        var modified = getLastWriteTime();

        if (Nullable.Equals(modified, lastModified))
            return false;

        if (resetModified)
            lastModified = modified;
        return true;
    }
}
