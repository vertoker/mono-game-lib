using Microsoft.Xna.Framework;
using SceneLib.Interfaces.Contexts;
using SceneLib.Core;

namespace SceneLib.Contexts
{
    public class SceneContext : ISceneContextSetup
    {
        private readonly GameContext _gameContext;
        private readonly Scene _scene;

        public GameContext GameContext => _gameContext;
        public Game Game => _gameContext.Game;

        public Scene Scene => _scene;
        public SceneTypeContainer Services => _scene.Services;

        public SceneContext(GameContext gameContext, Scene scene)
        {
            _gameContext = gameContext;
            _scene = scene;
        }

        public void AddService<T>(T service)
        {
            _scene.AddService(service);
        }
    }
}
