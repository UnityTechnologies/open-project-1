using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// A simple settings library with a built-in cache for global use.
/// </summary>
internal sealed class SettingsLibrary
{

    #region Singleton Setup

    private SettingsLibrary() { }
    private static readonly object instanceLock = new object();
    private static SettingsLibrary instance;

    /// <summary>
    /// The globally available instance of the settings library.
    /// </summary>
    /// <remarks>Utilizes coupled null reference checks surrounding a lock for thread safety.</remarks>
    public static SettingsLibrary Instance
    {
        get
        {
            if (instance == null)
                lock (instanceLock)
                    if (instance == null)
                        instance = new SettingsLibrary();

            return instance;
        }
    }

    #endregion

    #region Fields

    private Dictionary<string, object> settingsCache = new Dictionary<string, object>();

    #endregion

    #region Public Methods

    /// <summary>
    /// Attempt to get a setting from the library.
    /// </summary>
    /// <typeparam name="T">The resulting type of the requested setting.</typeparam>
    /// <param name="key">The key used to identify setting.</param>
    /// <param name="value">The value of the setting.</param>
    /// <returns>Returns true if the setting was found, otherwise false.</returns>
    public bool TryGetSetting<T>(string key, out T value)
    {
        value = default;
        try {
            lock (settingsCache) {
                if (settingsCache.ContainsKey(key)) {
                    value = (T)Convert.ChangeType(settingsCache[key], typeof(T));
                    return true;
                }

                // TODO: Create a method to load settings from a file.
                //    Access as an else-if to the above logical evaluation.
                //    Cache checking should always occur first for speed.
            }
        }
        catch { return false; }
        return false;
    }

    #endregion

}
