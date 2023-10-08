using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Render.Sprite
{
    public abstract class CustomGraphicsResource : IDisposable
    {
        private bool disposed;

        public GraphicsDevice GraphicsDevice { get; protected set; }

        public bool IsDisposed => disposed;

        public string Name { get; set; }

        public object Tag { get; set; }

        public event EventHandler<EventArgs> Disposing;

        public CustomGraphicsResource()
        {

        }

        ~CustomGraphicsResource()
        {
            Dispose(disposing: false);
        }

        protected internal virtual void GraphicsDeviceResetting()
        {

        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    Disposing(this, EventArgs.Empty);

                disposed = true;
            }
        }
    }
}
