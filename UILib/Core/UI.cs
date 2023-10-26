using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UILib.Extensions;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public class UI : IUIUpdate, IUIDraw
    {
        public readonly UIInput Input;
        public readonly UIContainer Container;

        public readonly HierarchyRenderBatch Batch;

        public readonly DepthEnumerator DepthEnumerator;

        public UI(HierarchyRenderBatch batch, int defaultDepth = 1)
        {
            Input = new UIInput();
            Container = new UIContainer(this);

            Batch = batch;

            DepthEnumerator = new DepthEnumerator(defaultDepth);

            Container.SetActive(true);
        }

        ~UI()
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
