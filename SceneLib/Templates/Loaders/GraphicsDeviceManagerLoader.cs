using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SceneLib.Interfaces.Kernel.Base;
using SceneLib.Contexts;
using System.Xml.Linq;
using SceneLib.Templates.Base.Loader;

namespace SceneLib.Templates.Loaders
{
    public class GraphicsDeviceManagerLoader : BaseValueHandler<GraphicsDeviceManager>, IServiceSetup, IContentLoad
    {
        private Game _game;

        public void Setup(SceneContext context)
        {
            _game = context.Game;
        }
        public void Load(ContentManager manager)
        {
            Value = new GraphicsDeviceManager(_game);
        }
    }
}
