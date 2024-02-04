
namespace TriangleNet.Rendering.Text
{
    using System.Drawing;

    /// <summary>
    /// Page size in millimeters.
    /// </summary>
    public struct PageSize
    {
        private const float MM_PER_INCH = 2.54f;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly PageSize A3 = new PageSize(297.0f, 420.0f);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly PageSize A4 = new PageSize(210.0f, 297.0f);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly PageSize A5 = new PageSize(148.0f, 210.0f);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly PageSize LETTER = new PageSize(8.5f * MM_PER_INCH, 11.0f * MM_PER_INCH);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly PageSize LEGAL = new PageSize(8.5f * MM_PER_INCH, 14.0f * MM_PER_INCH);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private float left;
        private float top;
        private float right;
        private float bottom;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public float X
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get { return left; }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public float Y
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get { return top; }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public float Width
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get { return right - left; }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public float Height
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get { return bottom - top; }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public float Right
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get { return right; }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public float Bottom
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get { return bottom; }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PageSize(float left, float top, float right, float bottom)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PageSize(float width, float height)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            : this(0.0f, 0.0f, width, height)
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PageSize(Rectangle size)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            : this(size.Left, size.Right, size.Top, size.Bottom)
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Expand(float dx, float dy)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            left -= dx;
            top -= dy;

            right += dx;
            bottom += dy;
        }
    }
}
