using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SceneLib.Interfaces.Contexts;
using SceneLib.Core;
using SceneLib.Interfaces.Setups;

namespace SceneLib.Contexts
{
    public class GameContext : IContextGame
    {
        private readonly SceneGame _game;

        public Game Game => _game;
        public ContentManager Content => _game.Content;

        public GameContext(SceneGame game)
        {
            _game = game;
        }

        public void AddProject<TScene>(TScene project) where TScene : ISceneSetup
        {
            var context = new SceneContext(this, _game.Project);
            project.Setup(context);
        }
        public void AddScene<TScene>(TScene scene) where TScene : ISceneSetup
        {
            var sceneItem = new Scene();
            _game.AddScene(typeof(TScene), sceneItem);
            var context = new SceneContext(this, sceneItem, _game.Project);
            scene.Setup(context);
        }
    }
}
