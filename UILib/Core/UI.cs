using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib;
using System;
using UILib.Extensions;
using UILib.Interfaces.Core;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib.Render;
using System.Collections.Generic;
using System.Reflection;

namespace UILib.Core
{
    public class UI : IUIUpdate, IUIDraw, IDisposable
    {
        public readonly UIContainer Container;
        public readonly ContentManager Content;
        public readonly HierarchyRenderBatch Batch;
        public readonly DepthEnumerator DepthEnumerator;

        private readonly List<IUIModule> _modules = new();

        public UI(Camera camera, ContentManager content, int defaultDepth = 1)
        {
            Container = new UIContainer(this);
            Content = content;

            var preset = new HierarchyRenderBatchPreset() { zPosition = 1 };
            Batch = new HierarchyRenderBatch(camera, preset);

            DepthEnumerator = new DepthEnumerator(defaultDepth);

            Container.SetActive(true);
        }
        public UI(HierarchyRenderBatch batch, ContentManager content, int defaultDepth = 1)
        {
            Container = new UIContainer(this);
            Content = content;
            Batch = batch;

            DepthEnumerator = new DepthEnumerator(defaultDepth);

            Container.SetActive(true);
        }
        ~UI()
        {
            Dispose();
        }
        public void Dispose()
        {
            foreach (var module in _modules)
                module.Dispose();
            _modules.Clear();

            Container.SetActive(false);
        }

        public void AddModule(IUIModule module)
        {
            module.Initialize(this);
            _modules.Add(module);
        }
        public void RemoveModule(IUIModule module)
        {
            _modules.Remove(module);
            module.Dispose();
        }

        public void Update(GameTime time)
        {
            Container.Update(time);

            foreach (var module in _modules)
                module.Update(time);
        }
        public void Draw(GameTime time)
        {
            DepthEnumerator.Reset();

            Batch.Begin();
            Container.Draw(time);
            Batch.End();

            Batch.Begin();
            foreach (var module in _modules)
                module.Draw(time);
            Batch.End();
        }
    }
}
