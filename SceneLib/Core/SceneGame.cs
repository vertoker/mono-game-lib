using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SceneFramework.Core.Model;
using SceneFramework.Interfaces.Kernel;
using System.Collections.Generic;
using System;
using static System.Formats.Asn1.AsnWriter;
using SceneFramework.Interfaces;

namespace SceneFramework.Core
{
    public class SceneGame : Game, ISceneSetup, ISceneManager
    {
        private readonly Dictionary<Type, BaseScene> _scenes;
        private readonly GameContext _context;

        private BaseScene _project;
        private BaseScene _activeScene;

        public SceneGame(IGameSetup setup)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _scenes = new Dictionary<Type, BaseScene>();
            _context = new GameContext()
            {
                Game = this,
                Content = Content
            };
            setup.Setup(this, _context);
        }

        protected override void Initialize()
        {
            _project.ServiceKernel.Initialize();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _project.LoaderKernel.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            _project.ServiceKernel.Update(gameTime);
            _activeScene?.ServiceKernel.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _project.ServiceKernel.Draw(gameTime);
            _activeScene?.ServiceKernel.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _activeScene?.LoaderKernel.Unload(Content);
            _project.LoaderKernel.Unload(Content);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _activeScene?.ServiceKernel.Dispose();
                _project.ServiceKernel.Dispose();
            }
            base.Dispose(disposing);
        }

        public void AddProject<TScene>(TScene project) where TScene : BaseScene
        {
            AddContext(project);
            _project = project;
        }
        public void AddScene<TScene>(TScene scene) where TScene : BaseScene
        {
            AddContext(scene);
            _scenes.Add(scene.GetType(), scene);
        }
        private void AddContext(BaseScene scene)
        {
            var context = new SceneContext()
            {
                Game = this
            };
            scene.Context = context;
        }

        public void Open<TScene>() where TScene : BaseScene
        {
            _activeScene?.ServiceKernel.Dispose();
            _activeScene?.LoaderKernel.Unload(Content);
            _activeScene = _scenes[typeof(TScene)];
            _activeScene?.LoaderKernel.Load(Content);
            _activeScene.ServiceKernel.Initialize();
        }
    }
}