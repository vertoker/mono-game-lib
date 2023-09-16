using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneFramework.Interfaces.Kernel
{
    public interface IKernelSetup
    {
        public void AddService(object service);
        public void AddServices(params object[] services)
        {
            foreach (var service in services)
                AddService(service);
        }
    }
}
