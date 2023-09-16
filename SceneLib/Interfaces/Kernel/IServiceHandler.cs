using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SceneLib.Interfaces.Kernel.Base;

namespace SceneLib.Interfaces.Kernel
{
    public interface IServiceHandler : IServiceInitializable, IServiceUpdateable, IServiceDrawable, IServiceDisposable
    {
    }
}
