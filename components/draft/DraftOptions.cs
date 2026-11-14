// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// Draft configuration options.
    /// </summary>
    public class DraftOptions
    {
        /// <summary>
        /// Gets or sets the unique identifier for the draft (used to distinguish different forms or data).
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the delay save duration in milliseconds. Default is 1000ms.
        /// </summary>
        public int DelayMilliseconds { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the recovery mode. Default is Confirm mode.
        /// </summary>
        public DraftRecoveryMode RecoveryMode { get; set; } = DraftRecoveryMode.Confirm;

        /// <summary>
        /// Gets or sets the current version number for version comparison.
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// Gets or sets whether version check is enabled. Default is true.
        /// </summary>
        public bool EnableVersionCheck { get; set; } = true;

        /// <summary>
        /// Gets or sets whether draft functionality is enabled. Default is true.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the custom storage provider (optional, defaults to LocalStorage).
        /// </summary>
        public IDraftStorageProvider StorageProvider { get; set; }

        /// <summary>
        /// Gets or sets the custom version comparison function.
        /// Returns true if the draft should be recovered (e.g., draft version > current version).
        /// Parameters: (draftVersion, currentVersion) => bool
        /// If not set, simple string equality check is used when EnableVersionCheck is true.
        /// </summary>
        public System.Func<string, string, bool> ShouldRecoverDraft { get; set; }
    }
}
