
namespace TriangleNet.Rendering.Buffer
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum BufferTarget : byte
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ColorBuffer,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        IndexBuffer,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        VertexBuffer
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IBuffer<T> where T : struct
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Gets the contents of the buffer.
        /// </summary>
        T[] Data { get; }

        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the size of one element in the buffer (i.e. 2 for 2D points
        /// or 3 for triangles).
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets the buffer target (vertices or indices).
        /// </summary>
        BufferTarget Target { get; }
    }
}
