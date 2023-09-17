using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Interfaces.Container
{
    public interface ITypeContainerWrite
    {
        public void Add<TService>(TService service) where TService : class;
        public void Add(Type type, object service);
        public void Remove<TService>() where TService : class;
        public void Remove(Type type);
    }
}
