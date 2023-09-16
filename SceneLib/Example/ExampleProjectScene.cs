using SceneLib.Templates.Loaders;
using SceneLib.Contexts;
using SceneLib.Interfaces.Setups;

namespace SceneLib.Example
{
    public class ExampleProjectScene : ISceneSetup
    {
        public void Setup(SceneContext context)
        {
            context.AddService(new GraphicsDeviceManagerLoader());
            context.AddService(new SpriteBatchLoader(context.Game));
        }
    }
}
