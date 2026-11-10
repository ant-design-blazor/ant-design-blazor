// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace AntDesign
{
    /// <summary>
    /// Defines the class-name tokens used to build overlay animation classes.
    /// </summary>
    public sealed class OverlayAnimationOptions
    {
        /// <summary>
        /// Gets or sets the class prefix, such as "ant".
        /// </summary>
        public string Prefix { get; set; } = "ant";

        /// <summary>
        /// Gets or sets the animation name, such as "slide" or "zoom-big".
        /// </summary>
        public string Animation { get; set; }

        /// <summary>
        /// Gets or sets a fixed direction token, such as "up". When omitted,
        /// <see cref="DirectionFactory"/> can select a direction from the placement.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets the enter phase token.
        /// </summary>
        public string Enter { get; set; } = "enter";

        /// <summary>
        /// Gets or sets the leave phase token.
        /// </summary>
        public string Leave { get; set; } = "leave";

        /// <summary>
        /// Gets or sets the active-state token appended to the phase token.
        /// </summary>
        public string Active { get; set; } = "active";

        /// <summary>
        /// Gets or sets a function that selects the direction token for a placement.
        /// Leave classes receive the reverse placement.
        /// </summary>
        public Func<Placement, string> DirectionFactory { get; set; }

        internal string GetEnterClass(Placement placement) => BuildClass(placement, Enter);

        internal string GetLeaveClass(Placement placement)
        {
            var reversePlacement = PlacementType.Create(placement).GetReverseType().Placement;
            return BuildClass(reversePlacement, Leave);
        }

        private string BuildClass(Placement placement, string phase)
        {
            if (string.IsNullOrEmpty(Animation))
            {
                return null;
            }

            var direction = Direction ?? DirectionFactory?.Invoke(placement);
            var animationClass = JoinTokens(Prefix, Animation, direction);
            var phaseClass = JoinTokens(animationClass, phase);
            var activeClass = JoinTokens(phaseClass, Active);
            return string.Join(" ", new[] { phaseClass, activeClass, animationClass }.Where(token => !string.IsNullOrWhiteSpace(token)));
        }

        private static string JoinTokens(params string[] tokens) => string.Join("-", tokens.Where(token => !string.IsNullOrWhiteSpace(token)));
    }
}
