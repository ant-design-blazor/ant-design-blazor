using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public sealed class TourActionsRenderContext
    {
        /// <summary>
        /// Zero-based current step index.
        /// </summary>
        public int Current { get; set; }
        /// <summary>
        /// Total number of tour steps.
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Default action buttons.
        /// </summary>
        public RenderFragment Origin { get; set; }
        /// <summary>
        /// Closes the tour.
        /// </summary>
        public Func<Task> Close { get; set; }
        /// <summary>
        /// Advances to the next step.
        /// </summary>
        public Func<Task> Next { get; set; }
        /// <summary>
        /// Returns to the previous step.
        /// </summary>
        public Func<Task> Previous { get; set; }
    }
}
