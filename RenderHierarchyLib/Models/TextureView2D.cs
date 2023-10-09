using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderHierarchyLib.Models
{
    public class TextureView2D
    {
        public Texture2D Texture { get; private set; }
        public Vector2 ViewStart { get; private set; } = Vector2.Zero;
        public Vector2 ViewEnd { get; private set; } = Vector2.One;
        public Color Color { get; private set; } = Color.White;

        public TextureView2D(Texture2D texture)
        {
            Texture = texture;
        }
        public TextureView2D(Texture2D texture, Rectangle rectangle)
        {
            Texture = texture;
            ViewStart = new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width);
            ViewEnd = new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width);
        }
        public TextureView2D(Texture2D texture, Vector2 viewStart, Vector2 viewEnd)
        {
            Texture = texture;
            ViewStart = viewStart;
            ViewEnd = viewEnd;
        }

        public TextureView2D(Texture2D texture, Color color)
        {
            Texture = texture;
            Color = color;
        }
        public TextureView2D(Texture2D texture, Color color, Rectangle rectangle)
        {
            Texture = texture;
            Color = color;
            ViewStart = new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width);
            ViewEnd = new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width);
        }
        public TextureView2D(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd)
        {
            Texture = texture;
            Color = color;
            ViewStart = viewStart;
            ViewEnd = viewEnd;
        }
    }
}
