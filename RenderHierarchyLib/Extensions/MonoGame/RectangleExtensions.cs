using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Extensions.MonoGame
{
    public static class RectangleExtensions
    {
        public static Vector2 TL(this Rectangle rectangle) => new(rectangle.Left, rectangle.Top);
        public static Vector2 TR(this Rectangle rectangle) => new(rectangle.Right, rectangle.Top);
        public static Vector2 BL(this Rectangle rectangle) => new(rectangle.Left, rectangle.Bottom);
        public static Vector2 BR(this Rectangle rectangle) => new(rectangle.Right, rectangle.Bottom);
    }
}
