using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SceneLib.Interfaces;
using SceneLib.Contexts;
using System;

namespace SceneLib.Core
{
    public class SceneGame : Game, ISceneManager
    {
        public readonly Dictionary<Type, Scene> Scenes;
        public Scene Project;

        public Scene ActiveScene;

        public SceneGame(IGameSetup setup)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Scenes = new Dictionary<Type, Scene>();
            var context = new GameContext(this);
            setup.Setup(context);
        }

        protected override void Initialize()
        {
            Project.ServiceKernel.Initialize();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Project.ServiceKernel.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Project.ServiceKernel.Update(gameTime);
            ActiveScene?.ServiceKernel.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            Project.ServiceKernel.Draw(gameTime);
            ActiveScene?.ServiceKernel.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            ActiveScene?.ServiceKernel.Unload(Content);
            Project.ServiceKernel.Unload(Content);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ActiveScene?.ServiceKernel.Dispose();
                Project.ServiceKernel.Dispose();
            }
            base.Dispose(disposing);
        }

        public void AddProject(Scene scene)
        {
            Project = scene;
        }
        public void AddScene(Scene scene)
        {
            Scenes.Add(scene.SceneType, scene);
        }

        public void Open<TScene>() where TScene : ISceneSetup
        {
            ActiveScene?.ServiceKernel.Dispose();
            ActiveScene?.ServiceKernel.Unload(Content);
            ActiveScene = Scenes[typeof(TScene)];
            ActiveScene.ServiceKernel.Load(Content);
            ActiveScene.ServiceKernel.Initialize();
        }
    }
}