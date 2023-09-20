using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib;

namespace TestingDesktop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private HierarchyRenderBatch _hierarchySpriteBatch;
        private Camera _camera;

        private SpriteBatch _spriteBatch;
        private Texture2D _texture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _camera = new Camera(new TransformCamera());
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
            _texture = Content.Load<Texture2D>("Test");

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

            _hierarchySpriteBatch.Begin();
            //_hierarchySpriteBatch.Draw(_texture, new Vector2(10, 10), null, Color.White, 0, Vector2.Zero, Vector2.One * 0.01f, SpriteEffects.None, 0);
            _hierarchySpriteBatch.RenderTest(_texture);
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