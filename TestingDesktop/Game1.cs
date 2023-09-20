using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestingDesktop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;
        private HierarchySpriteBatch _hierarchySpriteBatch;
        private Texture2D _texture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            _hierarchySpriteBatch = new HierarchySpriteBatch(GraphicsDevice);
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
            _hierarchySpriteBatch.Render(_texture);
            //_hierarchySpriteBatch.Draw(_texture, new Vector2(10, 10), null, Color.White, 0, new Vector2(0, 0), new Vector2(0.01f, 0.01f), SpriteEffects.None, 0);
            _hierarchySpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}