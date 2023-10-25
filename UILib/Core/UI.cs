using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public class UI : IUIUpdate, IUIDraw
    {
        public readonly UIInput Input;
        public readonly UIContainer Container;

        public UI(HierarchyRenderBatch batch)
        {
            Input = new UIInput();
            Container = new UIContainer();

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
            Container.Draw(time);
        }
    }
}
