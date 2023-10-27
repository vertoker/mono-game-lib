using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib;
using RenderHierarchyLib.Models.Transform;
using UILib.Core;
using UILib.Elements;

namespace UILibTesting
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _manager;
        private Camera _camera;

        private HierarchyRenderBatch _batch;
        private UI _ui;
        private ImageElement _image;

        public Game1()
        {
            _manager = new GraphicsDeviceManager(this);
            _camera = new Camera(new TransformCamera(10), _manager);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();

            _batch = new HierarchyRenderBatch(_manager.GraphicsDevice, _camera);
            _ui = new UI(_batch);

            var texture = Content.Load<Texture2D>("Test");
            _image = new ImageElement(texture);
            _ui.Container.AddChild(_image);
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _ui.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _image.LocalRotation = (float)gameTime.TotalGameTime.TotalSeconds * -5f;

            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
