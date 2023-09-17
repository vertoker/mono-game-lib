using SceneLib.Interfaces.Setups;

namespace SceneLib.Interfaces.Contexts
{
    public interface IContextGame
    {
        public void AddProject<TScene>(TScene scene) where TScene : ISceneSetup;
        public void AddScene<TScene>(TScene scene) where TScene : ISceneSetup;
    }
}
