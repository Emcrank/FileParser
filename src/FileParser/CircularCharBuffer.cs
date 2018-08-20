using System;
using System.Text;

namespace FileParser
{
    /// <summary>
    /// Circular buffer in which chars drop off the end when buffer is at max capacity.
    /// </summary>
    internal class CircularCharBuffer : CircularBuffer<char>
    {
        /// <summary>
        /// Gets the circular buffer as a string.
        /// </summary>
        internal string All
        {
            get
            {
                var sb = new StringBuilder(Capacity);
                foreach (char character in InternalBuffer)
                    sb.Append(character);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Constructs an instance of the circular character buffer.
        /// </summary>
        /// <param name="capacity"></param>
        internal CircularCharBuffer(int capacity) : base(capacity) { }

        /// <summary>
        /// New implementation that pushes the character to the buffer then returns All property as a
        /// return value of this method.
        /// </summary>
        /// <param name="character">Character to add to buffer.</param>
        /// <returns>All property</returns>
        internal new string Push(char character)
        {
            base.Push(character);
            return All;
        }

        /// <summary>
        /// Pushes a character to the buffer and returns a boolean value whether the given string matches.
        /// </summary>
        /// <param name="character">Character to add to buffer.</param>
        /// <param name="stringToMatch">String to match against buffer.</param>
        /// <returns>True if the string given matches the buffer.</returns>
        internal bool PushAndMatch(char character, string stringToMatch)
        {
            return Push(character).Equals(stringToMatch, StringComparison.Ordinal);
        }
    }
}