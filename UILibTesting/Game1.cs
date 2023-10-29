using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib;
using RenderHierarchyLib.Models.Transform;
using System.Xml.Linq;
using UILib.Core;
using UILib.Elements;
using UILib.Extensions;
using UILib.Modules;
using static System.Net.Mime.MediaTypeNames;

namespace UILibTesting
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _manager;

        private UI _ui;
        private Camera _camera;

        private ImageElement _image;
        private TextElement _text;

        public Game1()
        {
            _manager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _camera = new Camera(10, _manager);
            _ui = new UI(_camera, Content);

            var texture = Content.Load<Texture2D>("Empty");
            var font = Content.Load<SpriteFont>("RobotoFont");

            _ui.AddModule(new MouseInputModule(texture));

            _image = new ImageElement(texture);
            _text = new TextElement(font);

            _image.Color = Color.Black;
            _text.Text = "Test text";

            _ui.Container.AddChild(_image);
            _image.AddChild(_text);
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
