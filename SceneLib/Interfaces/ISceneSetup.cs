using SceneFramework.Core;

namespace SceneFramework.Interfaces
{
    public interface ISceneSetup
    {
        public void AddProject<TScene>(TScene scene) where TScene : BaseScene;
        public void AddScene<TScene>(TScene scene) where TScene : BaseScene;
    }
}
