using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SceneLib.Interfaces.Kernel.Base;

namespace SceneLib.Templates.Loaders
{
    public class GraphicsDeviceManagerLoader : IContentLoad
    {
        private readonly Game _game;

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public GraphicsDeviceManagerLoader(Game game)
        {
            _game = game;
        }
        public void Load(ContentManager manager)
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(_game);
        }
    }
}
