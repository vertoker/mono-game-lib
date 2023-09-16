namespace SceneLib.Interfaces.Contexts
{
    public interface ISceneContextSetup
    {
        public void AddService<T>(T service);
    }
}
