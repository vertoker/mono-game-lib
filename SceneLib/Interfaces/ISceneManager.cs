namespace SceneLib.Interfaces
{
    public interface ISceneManager
    {
        public void Open<TScene>() where TScene : ISceneSetup;
    }
}
