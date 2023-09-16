using Microsoft.Xna.Framework;
using SceneFramework.Interfaces.Kernel;
using SceneFramework.Interfaces.Kernel.Base;
using System;
using System.Collections.Generic;

namespace SceneFramework.Core.Model
{
    public class ServiceKernel : IKernelSetup, IServiceHandler
    {
        public List<IServiceInitializable> initializables = new();
        public List<IServiceUpdateable> updatables = new();
        public List<IServiceDrawable> drawables = new();
        public List<IServiceDisposable> disposeables = new();

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
    }
}
