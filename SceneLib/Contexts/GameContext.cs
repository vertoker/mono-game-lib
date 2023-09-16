using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SceneLib.Interfaces.Contexts;
using SceneLib.Core;
using SceneLib.Interfaces.Setups;

namespace SceneLib.Contexts
{
    public class GameContext : IGameContextSetup
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
            var sceneItem = new Scene(typeof(TScene));
            _game.AddProject(sceneItem);
            var context = new SceneContext(this, sceneItem);
            project.Setup(context);
        }
        public void AddScene<TScene>(TScene scene) where TScene : ISceneSetup
        {
            var sceneItem = new Scene(typeof(TScene));
            _game.AddScene(sceneItem);
            var context = new SceneContext(this, sceneItem);
            scene.Setup(context);

        }
    }
}
