using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Interfaces.Container
{
    public interface ITypeContainerRead
    {
        public TService Get<TService>() where TService : class;
        public object Get(Type type);
    }
}
