// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Locales
{
    public class DraftLocale
    {
        /// <summary>
        /// Title of the draft recovery confirmation dialog
        /// </summary>
        public string RecoveryTitle { get; set; } = "Draft Found";

        /// <summary>
        /// Content template of the draft recovery confirmation dialog.
        /// Supports placeholder: {time} for saved time
        /// </summary>
        public string RecoveryContent { get; set; } = "An unfinished draft was detected. Would you like to recover it?\n\nSaved at: {time}";

        /// <summary>
        /// Text for the draft recovery confirmation button
        /// </summary>
        public string RecoveryOkText { get; set; } = "Recover";

        /// <summary>
        /// Text for the draft deletion button
        /// </summary>
        public string RecoveryCancelText { get; set; } = "Delete";
    }
}
