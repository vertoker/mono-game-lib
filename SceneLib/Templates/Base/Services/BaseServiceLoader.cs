using Microsoft.Xna.Framework.Content;
using SceneLib.Contexts;
using SceneLib.Interfaces.Kernel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Templates.Base.Services
{
    public abstract class BaseServiceLoader<T> : IContentLoad, IContentUnload
    {
        public T Value { get; private set; }

        public abstract string AssetName { get; }

        public void Load(ContentManager manager)
        {
            Value = manager.Load<T>(AssetName);
        }
        public void Unload(ContentManager manager)
        {
            manager.UnloadAsset(AssetName);
        }
    }
}
