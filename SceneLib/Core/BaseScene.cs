using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SceneFramework.Core.Model;
using SceneFramework.Interfaces.Kernel;

namespace SceneFramework.Core
{
    public abstract class BaseScene
    {
        public readonly LoaderKernel LoaderKernel = new();
        public readonly ServiceKernel ServiceKernel = new();

        public SceneContext Context { get; set; }

        protected virtual void RegisterLoaders(IKernelSetup kernel) { }
        protected virtual void RegisterServices(IKernelSetup kernel) { }

        public BaseScene()
        {
            RegisterLoaders(LoaderKernel);
            RegisterServices(ServiceKernel);
        }
    }
}
