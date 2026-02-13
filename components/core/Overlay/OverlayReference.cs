// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// Reference to an opened overlay instance
    /// </summary>
    public class OverlayReference
    {
        private readonly Func<Task> _closeAction;
        private bool _isClosed;

        /// <summary>
        /// Unique identifier for this overlay instance
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Whether the overlay is closed
        /// </summary>
        public bool IsClosed => _isClosed;

        internal OverlayReference(string id, Func<Task> closeAction)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            _closeAction = closeAction ?? throw new ArgumentNullException(nameof(closeAction));
        }

        /// <summary>
        /// Close the overlay
        /// </summary>
        public async Task CloseAsync()
        {
            if (_isClosed) return;
            
            _isClosed = true;
            await _closeAction();
        }

        /// <summary>
        /// Close the overlay synchronously
        /// </summary>
        public void Close()
        {
            if (_isClosed) return;
            
            _isClosed = true;
            _ = _closeAction();
        }
    }
}
