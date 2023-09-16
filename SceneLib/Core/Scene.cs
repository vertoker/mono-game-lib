using System;
using System.ComponentModel.Design;

namespace SceneLib.Core
{
    public class Scene
    {
        public readonly SceneTypeContainer Services;
        public readonly SceneKernel Kernel;
        public readonly Type SceneType;

        public Scene(Type sceneType)
        {
            Services = new SceneTypeContainer();
            Kernel = new SceneKernel();
            SceneType = sceneType;
        }

        public void AddService<T>(T service)
        {
            Kernel.AddService(service);
            Services.Add(service);
        }
    }
}
