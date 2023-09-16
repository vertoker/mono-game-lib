using SceneFramework.Core;
using SceneFramework.Core.Model;
using SceneFramework.Interfaces.Kernel;
using SceneFramework.Templates.Loaders;

namespace SceneFramework.Example
{
    public class ProjectScene : BaseScene
    {
        protected override void RegisterLoaders(IKernelSetup kernel)
        {
            kernel.AddServices(
                new GraphicsDeviceManagerLoader(Context.Game),
                new SpriteBatchLoader(Context.Game));
        }
        protected override void RegisterServices(IKernelSetup kernel)
        {

        }
    }
}
