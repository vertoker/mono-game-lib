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

        public void Add<TService>(TService service)
        {
            Add(typeof(TService), service);
        }
        public void Add(Type type, object service)
        {
            _services.Add(type, service);
        }

        public TService Get<TService>()
        {
            return (TService)Get(typeof(TService));
        }
        public object Get(Type type)
        {
            return _services[type];
        }

        public void Remove<TService>()
        {
            Remove(typeof(TService));
        }
        public void Remove(Type type)
        {
            _services.Remove(type);
        }
    }
}
