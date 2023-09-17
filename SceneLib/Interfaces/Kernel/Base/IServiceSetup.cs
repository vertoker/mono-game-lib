using SceneLib.Contexts;
using SceneLib.Interfaces.Setups;

namespace SceneLib.Interfaces.Kernel.Base
{
    public interface IServiceSetup
    {
        public void Setup(ServiceContext context);
    }
}
