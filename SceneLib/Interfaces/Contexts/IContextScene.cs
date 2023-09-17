namespace SceneLib.Interfaces.Contexts
{
    public interface IContextScene : IContextService
    {
        public T Add<T>(T service) where T : class;

        public T AddKernel<T>(T service) where T : class;
        public T AddService<T>(T service) where T : class;
    }
}
