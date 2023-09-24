using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib;
using RenderHierarchyLib.Models;
using RenderHierarchyLib.Extensions;

namespace TestingDesktop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private HierarchyRenderBatch _hierarchySpriteBatch;
        private Camera _camera;

        private SpriteBatch _spriteBatch;
        private TextureView2D _texture1;
        private TextureView2D _texture2;
        private TextureView2D _texture3;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _camera = new Camera(new TransformCamera(10), _graphics);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _hierarchySpriteBatch = new HierarchyRenderBatch(GraphicsDevice, _camera);
            _texture1 = new TextureView2D(Content.Load<Texture2D>("Test"), new(0, 0.5f), new(1f, 1f));
            _texture2 = new TextureView2D(Content.Load<Texture2D>("Test2"));
            _texture3 = new TextureView2D(Content.Load<Texture2D>("Test3"));

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var counter = (float)gameTime.TotalGameTime.TotalSeconds * 50;

            _hierarchySpriteBatch.Begin();
            
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.LeftTop, Rot = counter });
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.CenterTop, Rot = counter });
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.RightTop, Rot = counter });
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.LeftMiddle, Rot = counter });
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.RightMiddle, Rot = counter });
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.LeftBottom, Rot = counter });
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.CenterBottom, Rot = counter });
            _hierarchySpriteBatch.Render(_texture3, new() { Anchor = AnchorPresets.RightBottom, Rot = counter });

            var parent = new RenderObject() { Anchor = AnchorPresets.CenterMiddle, Pivot = AnchorPresets.LeftTop, 
                Pos = new(0, 0), Rot = counter, Sca = new(2f, 1f) };
            _hierarchySpriteBatch.Render(_texture1, parent);

            var child = new RenderObject() { Anchor = AnchorPresets.CenterMiddle, Pivot = AnchorPresets.RightTop, Pos = new(0.5f, 0.5f), Rot = counter };
            var child2 = child.SetParentNewTransform(parent);
            _hierarchySpriteBatch.Render(_texture1, child2);
            var child3 = child.SetParentNewTransform(child2);
            _hierarchySpriteBatch.Render(_texture1, child3);
            var child4 = child.SetParentNewTransform(child3);
            _hierarchySpriteBatch.Render(_texture1, child4);

            _hierarchySpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}