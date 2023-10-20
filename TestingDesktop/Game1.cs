using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib;
using RenderHierarchyLib.Models;
using RenderHierarchyLib.Extensions;
using RenderHierarchyLib.Models.Text;
using System.Diagnostics;
using System;

namespace TestingDesktop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private HierarchyRenderBatch _hierarchySpriteBatch;
        private Camera _camera;

        private SpriteBatch _spriteBatch;

        private TextView _font1;
        private TextureView[] _textures;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this) { GraphicsProfile = GraphicsProfile.HiDef, PreferMultiSampling = true };
            _graphics.PreparingDeviceSettings += (object sender, PreparingDeviceSettingsEventArgs e) =>
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 16;
            _graphics.ApplyChanges();

            _camera = new Camera(new TransformCamera(new(0, 0), 0, 10), _graphics);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

            GraphicsDevice.RasterizerState = new RasterizerState
            { CullMode = CullMode.CullClockwiseFace, MultiSampleAntiAlias = true };
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _hierarchySpriteBatch = new HierarchyRenderBatch(GraphicsDevice, _camera);
            var font = Content.Load<SpriteFont>("TextTest");
            _font1 = new TextView("Test text", font);
            _textures = new TextureView[]
            {
                new TextureView(Content.Load<Texture2D>("Test0")),
                new TextureView(Content.Load<Texture2D>("Test1"), new Vector2(0, 0.5f), new Vector2(1, 1)),
                new TextureView(Content.Load<Texture2D>("Test2")),
                new TextureView(Content.Load<Texture2D>("Test3")),
                new TextureView(Content.Load<Texture2D>("Test4"))
            };
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

            RenderText(gameTime);
            Render(gameTime);

            base.Draw(gameTime);
        }

        private bool single = false;
        private void RenderText(GameTime gameTime)
        {
            var counter = (float)gameTime.TotalGameTime.TotalSeconds * 0;

            if (single) return;
            //single = true;

            var text = "test1\ntest12\ntest123";

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_font1.Font.DefaultFont, text, new Vector2(500, 200), Color.Green,//Tes\rt \n textfghfhfhghfgh
                counter * MathExtensions.Deg2Rad, new Vector2(1, 1), new Vector2(1, 1), SpriteEffects.None, 1);

            _spriteBatch.End();

            _hierarchySpriteBatch.Begin();
            //_hierarchySpriteBatch.RenderTextTest(_font1.Font, "Tes\rt \n textfghfhfhghfgh");

            _hierarchySpriteBatch.DrawString(_font1.Font, text, new Vector2(200, 200), Color.Yellow,//Tes\rt \n textfghfhfhghfgh
                counter * MathExtensions.Deg2Rad, new Vector2(1, 1), new Vector2(1, 1), SpriteEffects.None, 1);

            //_hierarchySpriteBatch.CameraTextRender(_font1.Font, text, Color.Red, null, new Vector2(200, 200), 0,
            //    Vector2.One, Vector2.Zero, Vector2.Zero, 0, TextAlignmentHorizontal.Center);

            _hierarchySpriteBatch.End();
        }
        private void Render(GameTime gameTime)
        {
            var counter = (float)gameTime.TotalGameTime.TotalSeconds * 15;
            var counter2 = (float)gameTime.TotalGameTime.TotalSeconds * 1;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_textures[0].Texture, new Vector2(300, 100), new Rectangle(0, 0, 100, 100), Color.White, counter2, 
                new Vector2(0.5f, 0.5f), Vector2.One, SpriteEffects.None, 0);
            _spriteBatch.End();

            _hierarchySpriteBatch.Begin();

            _hierarchySpriteBatch.CameraRender(_textures[3], new() { Anchor = AnchorPresets.LeftTop, Rot = counter });
            _hierarchySpriteBatch.WorldRender(_textures[3], new() { Anchor = AnchorPresets.CenterTop, Rot = counter });
            _hierarchySpriteBatch.CameraRender(_textures[3], new() { Anchor = AnchorPresets.RightTop, Rot = counter });
            _hierarchySpriteBatch.WorldRender(_textures[3], new() { Anchor = AnchorPresets.LeftMiddle, Rot = counter });
            _hierarchySpriteBatch.WorldRender(_textures[3], new() { Anchor = AnchorPresets.RightMiddle, Rot = counter });
            _hierarchySpriteBatch.CameraRender(_textures[3], new() { Anchor = AnchorPresets.LeftBottom, Rot = counter });
            _hierarchySpriteBatch.WorldRender(_textures[3], new() { Anchor = AnchorPresets.CenterBottom, Rot = counter });
            _hierarchySpriteBatch.CameraRender(_textures[3], new() { Anchor = AnchorPresets.RightBottom, Rot = counter });

            var parent = new RenderObject()
            {
                Anchor = AnchorPresets.CenterMiddle,
                Pivot = AnchorPresets.CenterMiddle,
                Pos = new(0, 0),
                Rot = counter,
                Sca = new(2f, 2f)
            };
            //_hierarchySpriteBatch.WorldRender(_textures[0], parent);

            //_hierarchySpriteBatch.RenderTest(_textures[0].Texture);

            _camera.Transform.Rot = 0;// counter2;

            var child = new RenderObject() { Anchor = AnchorPresets.CenterMiddle, Pivot = AnchorPresets.RightTop, Pos = new(0.5f, 0.5f), Rot = counter };
            var child2 = child.SetParentNewTransform(parent);
            //_hierarchySpriteBatch.WorldRender(_textures[1], child2);
            var child3 = child.SetParentNewTransform(child2);
            //_hierarchySpriteBatch.WorldRender(_textures[1], child3);
            var child4 = child.SetParentNewTransform(child3);
            //_hierarchySpriteBatch.WorldRender(_textures[1], child4);

            _hierarchySpriteBatch.End();
        }
    }
}