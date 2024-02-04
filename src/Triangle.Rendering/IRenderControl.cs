// -----------------------------------------------------------------------
// <copyright file="IMeshRenderer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering
{
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRenderControl
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        IRenderer Renderer { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Rectangle ClientRectangle { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void Initialize();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void Refresh();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void HandleResize();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
