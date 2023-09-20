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
        public Vector2 ViewStart { get; private set; }
        public Vector2 ViewEnd { get; private set; }

        public TextureView2D(Texture2D texture) : this(texture, Vector2.Zero, Vector2.One)
        {
            Texture = texture;
        }
        public TextureView2D(Texture2D texture, Vector2 viewStart, Vector2 viewEnd)
        {
            Texture = texture;
            ViewStart = viewStart;
            ViewEnd = viewEnd;
        }
        public TextureView2D(Texture2D texture, Rectangle rectangle)
        {
            Texture = texture;
            ViewStart = new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width);
            ViewEnd = new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width);
        }
    }
}
