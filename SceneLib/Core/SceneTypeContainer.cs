using SceneLib.Interfaces.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Core
{
    public class SceneTypeContainer : ITypeContainer
    {
        private Dictionary<Type, object> _services = new();

        public void Add<TService>(TService service) where TService : class
        {
            Add(typeof(TService), service);
        }
        public void Add(Type type, object service)
        {
            _services.Add(type, service);
        }

        public TService Get<TService>() where TService : class
        {
            var service = Get(typeof(TService));
            return service == null ? null : (TService)service;
        }
        public object Get(Type type)
        {
            if (_services.TryGetValue(type, out var service))
                return service;
            return null;
        }

        public void Remove<TService>() where TService : class
        {
            Remove(typeof(TService));
        }
        public void Remove(Type type)
        {
            _services.Remove(type);
        }
    }
}
