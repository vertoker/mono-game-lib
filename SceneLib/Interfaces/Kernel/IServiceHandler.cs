using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SceneFramework.Interfaces.Kernel.Base;

namespace SceneFramework.Interfaces.Kernel
{
    public interface IServiceHandler : IServiceInitializable, IServiceUpdateable, IServiceDrawable, IServiceDisposable
    {
    }
}
