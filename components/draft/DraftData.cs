// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    /// <summary>
    /// Draft data wrapper class.
    /// </summary>
    /// <typeparam name="T">The data type</typeparam>
    public class DraftData<T>
    {
        /// <summary>
        /// Gets or sets the draft data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the saved time.
        /// </summary>
        public DateTime SavedAt { get; set; }

        /// <summary>
        /// Gets or sets the draft key.
        /// </summary>
        public string Key { get; set; }
    }
}
