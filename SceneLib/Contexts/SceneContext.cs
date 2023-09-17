using Microsoft.Xna.Framework;
using SceneLib.Interfaces.Contexts;
using SceneLib.Core;
using SceneLib.Interfaces.Kernel.Base;
using System;

namespace SceneLib.Contexts
{
    public class SceneContext : IContextScene
    {
        private readonly Scene _project;

        public GameContext GameContext { get; }
        public Scene Scene { get; }

        public Game Game => GameContext.Game;

        public SceneContext(GameContext gameContext, Scene scene) : this(gameContext, scene, null) { }
        public SceneContext(GameContext gameContext, Scene scene, Scene project)
        {
            GameContext = gameContext;
            Scene = scene;
            _project = project;
        }

        public T Add<T>(T service) where T : class
        {
            AddKernel(service);
            AddService(service);
            return service;
        }
        public T AddKernel<T>(T service) where T : class
        {
            Scene.Kernel.AddService(service);
            return service;
        }
        public T AddService<T>(T service) where T : class
        {
            Scene.Services.Add(service);
            if (service is IServiceSetup setup)
                setup.Setup(new ServiceContext(this));
            return service;
        }

        public T Get<T>(bool scene = true, bool project = true) where T : class
        {
            var service = scene ? GetInScene<T>() : null;
            return (project && service == null) ? GetInProject<T>() : service;
        }
        public T GetInScene<T>() where T : class
        {
            return Scene.Services.Get<T>();
        }
        public T GetInProject<T>() where T : class
        {
            return _project.Services.Get<T>();
        }
    }
}
