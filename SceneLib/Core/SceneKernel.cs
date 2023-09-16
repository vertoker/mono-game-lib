using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SceneLib.Contexts;
using SceneLib.Interfaces.Contexts;
using SceneLib.Interfaces.Kernel;
using SceneLib.Interfaces.Kernel.Base;
using System.Collections.Generic;

namespace SceneLib.Core
{
    public class SceneKernel : IServiceHandler, IContentHandler
    {
        public List<IServiceInitializable> initializables = new();
        public List<IServiceUpdateable> updatables = new();
        public List<IServiceDrawable> drawables = new();
        public List<IServiceDisposable> disposeables = new();

        public List<IContentLoad> loaders = new();
        public List<IContentUnload> unloaders = new();

        public void AddService(object service)
        {
            if (service is IServiceInitializable initializable)
                initializables.Add(initializable);
            if (service is IServiceUpdateable updateable)
                updatables.Add(updateable);
            if (service is IServiceDrawable drawable)
                drawables.Add(drawable);
            if (service is IServiceDisposable disposable)
                disposeables.Add(disposable);

            if (service is IContentLoad load)
                loaders.Add(load);
            if (service is IContentUnload unload)
                unloaders.Add(unload);
        }

        public void Initialize()
        {
            foreach (var item in initializables)
                item.Initialize();
        }
        public void Update(GameTime time)
        {
            foreach (var item in updatables)
                item.Update(time);
        }
        public void Draw(GameTime time)
        {
            foreach (var item in drawables)
                item.Draw(time);
        }
        public void Dispose()
        {
            foreach (var item in disposeables)
                item.Dispose();
        }

        public void Load(ContentManager manager)
        {
            foreach (var item in loaders)
                item.Load(manager);
        }
        public void Unload(ContentManager manager)
        {
            foreach (var item in unloaders)
                item.Unload(manager);
        }
    }
}
