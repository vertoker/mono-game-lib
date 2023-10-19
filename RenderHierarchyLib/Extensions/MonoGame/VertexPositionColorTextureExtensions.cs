using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderHierarchyLib.Extensions.MonoGame
{
    public static class VertexPositionColorTextureExtensions
    {
        public static void Setup(this VertexPositionColorTexture vertex, Vector3 position, Color color, Vector2 textureCoordinate)
        {
            vertex.Position = position;
            vertex.Color = color;
            vertex.TextureCoordinate = textureCoordinate;
        }
    }
}
