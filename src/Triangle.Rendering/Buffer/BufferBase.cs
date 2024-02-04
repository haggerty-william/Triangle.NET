
namespace TriangleNet.Rendering.Buffer
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class BufferBase<T> : IBuffer<T> where T : struct
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected T[] data;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected int size;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public BufferBase(int capacity, int size)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            : this(new T[capacity], size)
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public BufferBase(T[] data, int size)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.data = data;
            this.size = size;
        }

        /// <inheritdoc/>
        public T[] Data => data;

        /// <inheritdoc/>
        public int Count => data == null ? 0 : data.Length;

        /// <inheritdoc/>
        public abstract int Size { get; }

        /// <inheritdoc/>
        public abstract BufferTarget Target { get; }
    }
}
