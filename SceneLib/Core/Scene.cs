using System;
using System.ComponentModel.Design;

namespace SceneLib.Core
{
    public class Scene
    {
        public readonly SceneTypeContainer Services;
        public readonly SceneKernel Kernel;

        public Scene()
        {
            Services = new SceneTypeContainer();
            Kernel = new SceneKernel();
        }
    }
}
