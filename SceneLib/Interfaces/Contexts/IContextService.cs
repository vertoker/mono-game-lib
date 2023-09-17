using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Interfaces.Contexts
{
    public interface IContextService
    {
        public T Get<T>(bool scene = true, bool project = true) where T : class;
        public T GetInScene<T>() where T : class;
        public T GetInProject<T>() where T : class;
    }
}
