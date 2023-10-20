using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingDesktop
{
    public class GameTestAA : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;

        public GameTestAA()
        {
            graphics = new GraphicsDeviceManager(this) { GraphicsProfile = GraphicsProfile.Reach, PreferMultiSampling = false };
            graphics.PreparingDeviceSettings += (object sender, PreparingDeviceSettingsEventArgs e) => e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 16;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None, MultiSampleAntiAlias = true };
            GraphicsDevice.BlendState = new BlendState() { AlphaSourceBlend = Blend.SourceAlpha, 
                AlphaDestinationBlend = Blend.InverseSourceColor, ColorSourceBlend = Blend.SourceAlpha, 
                ColorDestinationBlend = Blend.InverseSourceAlpha };
            
            effect = new BasicEffect(GraphicsDevice) { VertexColorEnabled = true };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, new VertexPositionColor[] {
                    new VertexPositionColor(new Vector3(0.5f, 0, 0), Color.White),
                    new VertexPositionColor(new Vector3(0, 0.5f, 0), Color.White),
                    new VertexPositionColor(new Vector3(0, -0.5f, 0), Color.White)
                }, 0, 1);
            }

            base.Draw(gameTime);
        }
    }
}
