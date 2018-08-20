using System;
using System.Collections.Generic;

namespace NeatParser
{
    /// <summary>
    /// Generic abstract circular buffer class in which items drop off the end when buffer reaches
    /// max capacity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class CircularBuffer<T>
    {
        protected readonly int Capacity;
        protected readonly Queue<T> InternalBuffer;

        /// <summary>
        /// Constructs a new instance of the <see cref="CircularBuffer{T}"/>
        /// </summary>
        /// <param name="capacity">Total capacity.</param>
        internal CircularBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            Capacity = capacity;
            InternalBuffer = new Queue<T>(capacity);
        }

        /// <summary>
        /// Pushes a new item into the buffer.
        /// </summary>
        /// <param name="item">Item to add to buffer.</param>
        internal void Push(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (InternalBuffer.Count == Capacity)
                InternalBuffer.Dequeue();

            InternalBuffer.Enqueue(item);
        }
    }
}