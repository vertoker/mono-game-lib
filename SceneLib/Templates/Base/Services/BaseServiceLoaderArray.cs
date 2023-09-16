using Microsoft.Xna.Framework.Content;
using SceneLib.Interfaces.Kernel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Templates.Base.Services
{
    public abstract class BaseServiceLoaderArray<T> : IContentLoad, IContentUnload
    {
        public T[] Values { get; private set; }

        public abstract string[] AssetNames { get; }

        public void Load(ContentManager manager)
        {
            var names = AssetNames;
            Values = new T[names.Length];
            for (int i = 0; i < names.Length; i++)
                Values[i] = manager.Load<T>(names[i]);
        }
        public void Unload(ContentManager manager)
        {
            manager.UnloadAssets(AssetNames);
        }
    }
}
