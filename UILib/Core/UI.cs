using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib;
using System;
using UILib.Extensions;
using UILib.Interfaces.Core;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib.Render;

namespace UILib.Core
{
    public class UI : IUIUpdate, IUIDraw, IDisposable
    {
        public readonly UIInput Input;
        public readonly UIContainer Container;

        public readonly HierarchyRenderBatch Batch;
        public readonly ContentManager Content;

        public readonly DepthEnumerator DepthEnumerator;

        public UI(Camera camera, ContentManager content, int defaultDepth = 1)
        {
            Input = new UIInput();
            Container = new UIContainer(this);
            Content = content;

            var preset = new HierarchyRenderBatchPreset() { zPosition = 1 };
            Batch = new HierarchyRenderBatch(camera, preset);

            DepthEnumerator = new DepthEnumerator(defaultDepth);

            Container.SetActive(true);
        }
        public UI(HierarchyRenderBatch batch, ContentManager content, int defaultDepth = 1)
        {
            Input = new UIInput();
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
            Container.SetActive(false);
        }

        public void Update(GameTime time)
        {
            Input.Update(time);
            Container.Update(time);
        }
        public void Draw(GameTime time)
        {
            Input.Draw(time);

            DepthEnumerator.Reset();
            Batch.Begin();
            Container.Draw(time);
            Batch.End();
        }
    }
}
