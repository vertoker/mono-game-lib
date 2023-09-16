using SceneFramework.Core;

namespace SceneFramework.Interfaces
{
    public interface ISceneManager
    {
        public void Open<TScene>() where TScene : BaseScene;
    }
}
