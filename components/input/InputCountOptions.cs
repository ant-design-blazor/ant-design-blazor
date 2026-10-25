using System;

namespace AntDesign
{
    /// <summary>
    /// Configures the character counter for an input.
    /// </summary>
    public class InputCountOptions
    {
        /// <summary>
        /// Whether to display the character counter.
        /// </summary>
        public bool Show { get; set; } = true;

        /// <summary>
        /// The maximum count displayed by the counter.
        /// This does not limit input unless <see cref="ExceedFormatter"/> is provided.
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// Calculates the count from the input text. Defaults to UTF-16 string length.
        /// </summary>
        public Func<string, int> Strategy { get; set; }

        /// <summary>
        /// Formats input text when its count exceeds <see cref="Max"/>.
        /// The callback receives the original text and the configured maximum.
        /// </summary>
        public Func<string, int, string> ExceedFormatter { get; set; }
    }
}
