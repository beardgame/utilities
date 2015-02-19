using System;
using System.IO;

namespace Bearded.Utilities
{
    /// <summary>
    /// Enumerator for platforms.
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Windows platforms.
        /// </summary>
        Windows,
        /// <summary>
        /// Linux platforms.
        /// </summary>
        Linux,
        /// <summary>
        /// Mac platforms (OSX).
        /// </summary>
        OSX
    }

    /// <summary>
    /// Collection of platform-specific functions.
    /// </summary>
    public static class Environment
    {
        /// <summary>
        /// The platform the application is currently running on.
        /// </summary>
        public static Platform CurrentPlatform
        {
            get
            {
                switch (System.Environment.OSVersion.Platform)
                {
                    case PlatformID.Unix:
                        // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                        // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                        if (Directory.Exists("/Applications")
                            & Directory.Exists("/System")
                            & Directory.Exists("/Users")
                            & Directory.Exists("/Volumes"))
                            return Platform.OSX;
                        else
                            return Platform.Linux;

                    case PlatformID.MacOSX:
                        return Platform.OSX;

                    default:
                        return Platform.Windows;
                }
            }
        }

        /// <summary>
        /// Gets the default user settings directory for the current platform.
        /// For Windows: %appdata%
        /// For OSX: ~/Library/Application Support
        /// For Linux: [not implemented]
        /// </summary>
        public static string UserSettingsDirectory
        {
            get
            {
                switch(Environment.CurrentPlatform)
                {
                    case Platform.Windows:
                        return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                    case Platform.OSX:
                        return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                                "Library", "Application Support");
                    case Platform.Linux:
                        throw new NotImplementedException();
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }
}