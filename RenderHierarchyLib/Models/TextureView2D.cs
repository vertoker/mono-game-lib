using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Models
{
    public class TextureView2D
    {
        public Texture2D Texture { get; private set; }
        public Vector2 ViewStart { get; private set; } = Vector2.Zero;
        public Vector2 ViewEnd { get; private set; } = Vector2.One;

        public Color ColorTL { get; private set; } = Color.White;
        public Color ColorTR { get; private set; } = Color.White;
        public Color ColorBL { get; private set; } = Color.White;
        public Color ColorBR { get; private set; } = Color.White;

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
            : this(texture, color, color, color, color) { }
        public TextureView2D(Texture2D texture, Color colorTL, Color colorTR, Color colorBL, Color colorBR)
        {
            Texture = texture;
            ColorTL = colorTL;
            ColorTR = colorTR;
            ColorBL = colorBL;
            ColorBR = colorBR;
        }
        public TextureView2D(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color)
            : this(texture, viewStart, viewEnd, color, color, color, color) { }
        public TextureView2D(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, 
            Color colorTL, Color colorTR, Color colorBL, Color colorBR)
        {
            Texture = texture;
            ViewStart = viewStart;
            ViewEnd = viewEnd;
            ColorTL = colorTL;
            ColorTR = colorTR;
            ColorBL = colorBL;
            ColorBR = colorBR;
        }
    }
}
