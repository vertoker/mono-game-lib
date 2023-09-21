using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib;
using RenderHierarchyLib.Models;
using RenderHierarchyLib.Models.Enum;

namespace TestingDesktop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private HierarchyRenderBatch _hierarchySpriteBatch;
        private Camera _camera;

        private SpriteBatch _spriteBatch;
        private TextureView2D _texture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _camera = new Camera(new TransformCamera(10), _graphics);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _hierarchySpriteBatch = new HierarchyRenderBatch(GraphicsDevice, _camera);
            _texture = new TextureView2D(Content.Load<Texture2D>("Test3"), Color.Red, Color.Red, Color.Green, Color.Blue);

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

            var counter = (float)gameTime.TotalGameTime.TotalSeconds * 25;

            _hierarchySpriteBatch.Begin();
            //_hierarchySpriteBatch.Draw(_texture, new Vector2(10, 10), null, Color.White, 0, Vector2.Zero, Vector2.One * 0.01f, SpriteEffects.None, 0);
            
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Left_Top, Rot = counter });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Center_Top, Rot = counter });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Right_Top, Rot = counter });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Left_Middle, Rot = counter });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Center_Middle, Rot = counter, Sca = new(2, 1) });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Right_Middle, Rot = counter });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Left_Bottom, Rot = counter });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Center_Bottom, Rot = counter });
            _hierarchySpriteBatch.Render(_texture, new RenderTransformObject() { Anchor = Anchor.Right_Bottom, Rot = counter });

            //_hierarchySpriteBatch.RenderTest(_texture.Texture, counter);
            _hierarchySpriteBatch.End();


            /*
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, new Vector2(20,20), Color.White);
            _spriteBatch.End();
            */

            base.Draw(gameTime);
        }
    }
}