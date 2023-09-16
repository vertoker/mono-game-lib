using System;

namespace SceneLib.Core
{
    public class Scene
    {
        public readonly SceneKernel ServiceKernel;
        public readonly Type SceneType;

        public Scene(Type sceneType)
        {
            ServiceKernel = new SceneKernel();
            SceneType = sceneType;
        }
    }
}
