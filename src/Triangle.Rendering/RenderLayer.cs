
namespace TriangleNet.Rendering
{
    using TriangleNet.Rendering.Buffer;
    using TriangleNet.Rendering.Util;

    using Color = System.Drawing.Color;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class RenderLayer : IRenderLayer
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        int count;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected IBuffer<float> points;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected IBuffer<uint> indices;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected IBuffer<uint> partition;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected IBuffer<Color> colors;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderLayer"/> class.
        /// </summary>
        public RenderLayer()
        {
            IsEnabled = false;
        }

        /// <inheritdoc />
        public int Count => count;

        /// <inheritdoc />
        public IBuffer<float> Points => points;

        /// <inheritdoc />
        public IBuffer<uint> Indices => indices;

        /// <inheritdoc />
        public IBuffer<uint> Partition => partition;

        /// <inheritdoc />
        public IBuffer<Color> Colors => colors;

        /// <inheritdoc />
        public bool IsEnabled { get; set; }

        /// <inheritdoc />
        public bool IsEmpty()
        {
            return (points == null || points.Count == 0);
        }

        /// <inheritdoc />
        public void Reset(bool clear)
        {
            if (clear)
            {
                count = 0;
                points = null;
            }

            indices = null;
            partition = null;
            colors = null;
        }

        /// <inheritdoc />
        public void SetPoints(IBuffer<float> buffer, bool reset = true)
        {
            if (!reset && points != null && points.Count < buffer.Count)
            {
                // NOTE: we keep the old size to be able to render new Steiner
                //       points in a different color than existing points.
                count = points.Count / points.Size;
            }
            else
            {
                count = buffer.Count / buffer.Size;
            }

            points = buffer;
        }

        /// <inheritdoc />
        public void SetIndices(IBuffer<uint> buffer)
        {
            indices = buffer;
        }

        /// <inheritdoc />
        public void AttachLayerData(float[] values, ColorMap colormap)
        {
            var colorData = new Color[values.Length];

            colormap.GetColors(values, colorData);

            colors = new ColorBuffer(colorData, 1);
        }

        /// <inheritdoc />
        public void AttachLayerData(uint[] partition)
        {
            this.partition = new IndexBuffer(partition, 1);
        }
    }
}
