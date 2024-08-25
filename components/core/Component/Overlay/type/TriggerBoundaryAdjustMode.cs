// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public enum TriggerBoundaryAdjustMode
    {
        /// <summary>
        /// Do not auto adjust
        /// </summary>
        None,

        /// <summary>
        /// The default, the viewport boundaries are the boundaries that are used for calculation if overlay 
        /// is fully visible.
        /// Attempt to fit the overlay so it is always fully visible in the viewport.
        /// So if the overlay is outside of the viewport ("overflows"), then reposition calculation is going 
        /// to be attempted.
        /// </summary>
        InView,
        
        /// <summary>
        /// The document boundaries are the boundaries used for calculation if overlay needs to be reposition. 
        /// So even if the overlay is outside of the viewport, the overlay may still be shown as long as it 
        /// does not "overflow" the document boundaries.
        /// </summary>
        InScroll
    }
}
