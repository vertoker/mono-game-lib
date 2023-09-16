using SceneLib.Interfaces;
using SceneLib.Templates.Loaders;
using SceneLib.Contexts;

namespace SceneLib.Example
{
    public class ExampleProjectScene : ISceneSetup
    {
        public void Setup(SceneContext context)
        {
            context.AddService(new GraphicsDeviceManagerLoader(context.Game));
            context.AddService(new SpriteBatchLoader(context.Game));
        }
    }
}
