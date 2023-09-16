using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SceneFramework.Interfaces.Kernel.Base;

namespace SceneFramework.Templates.Loaders
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
