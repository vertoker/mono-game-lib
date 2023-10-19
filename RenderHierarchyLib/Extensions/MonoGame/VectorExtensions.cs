using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Extensions.MonoGame
{
    public static class VectorExtensions
    {
        public static Vector3 Plus(this Vector3 source, Vector2 plus)
        {
            source.X += plus.X;
            source.Y += plus.Y;
            return source;
        }
    }
}
