using Microsoft.Xna.Framework;
using SceneLib.Interfaces.Contexts;
using SceneLib.Core;

namespace SceneLib.Contexts
{
    public class SceneContext : ISceneContextSetup
    {
        private readonly SceneGame _game;
        private readonly Scene _scene;

        public Game Game => _game;

        public SceneContext(SceneGame game, Scene scene)
        {
            _game = game;
            _scene = scene;
        }

        public void AddService(object service)
        {
            _scene.ServiceKernel.AddService(service);
            _game.Services.AddService(service);
        }
        public void AddServices(params object[] services)
        {
            foreach (object service in services)
            {
                _scene.ServiceKernel.AddService(service);
                _game.Services.AddService(service);
            }
        }
    }
}
