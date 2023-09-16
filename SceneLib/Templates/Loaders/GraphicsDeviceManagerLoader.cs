using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SceneLib.Interfaces.Kernel.Base;
using SceneLib.Contexts;
using System.Xml.Linq;

namespace SceneLib.Templates.Loaders
{
    public class GraphicsDeviceManagerLoader : IServiceSetup, IContentLoad
    {
        private Game _game;

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public void Setup(SceneContext context)
        {
            _game = context.Game;
        }
        public void Load(ContentManager manager)
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(_game);
        }
    }
}
