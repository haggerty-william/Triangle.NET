
namespace TriangleNet.Rendering
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IRenderer
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        IRenderContext Context { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void Render();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
