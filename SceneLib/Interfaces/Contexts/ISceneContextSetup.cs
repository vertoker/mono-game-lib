namespace SceneLib.Interfaces.Contexts
{
    public interface ISceneContextSetup
    {
        public void AddService(object service);
        public void AddServices(params object[] services);
    }
}
