using Microsoft.Xna.Framework;
using SceneLib.Core;
using SceneLib.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Contexts
{
    public class ServiceContext : IContextService
    {
        private readonly SceneContext _sceneContext;
        private readonly SceneTypeContainer _services;

        public SceneContext SceneContext => _sceneContext;
        public Game Game => _sceneContext.Game;

        public ServiceContext(SceneContext sceneContext)
        {
            _sceneContext = sceneContext;
            _services = sceneContext.Scene.Services;
        }

        public T Get<T>(bool scene = true, bool project = true) where T : class
        {
            return _sceneContext.Get<T>(scene, project);
        }
        public T GetInScene<T>() where T : class
        {
            return _sceneContext.GetInScene<T>();
        }
        public T GetInProject<T>() where T : class
        {
            return _sceneContext.GetInProject<T>();
        }
    }
}
