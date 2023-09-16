using Microsoft.Xna.Framework.Content;
using SceneFramework.Interfaces.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneFramework.Core.Model
{
    public class LoaderKernel : IKernelSetup, IContentHandler
    {
        public List<IContentHandler> loaders = new();

        public void AddService(object service)
        {
            if (service is IContentHandler loader)
                loaders.Add(loader);
        }

        public void Load(ContentManager manager)
        {
            foreach (var item in loaders)
                item.Load(manager);
        }
        public void Unload(ContentManager manager)
        {
            foreach (var item in loaders)
                item.Unload(manager);
        }
    }
}
