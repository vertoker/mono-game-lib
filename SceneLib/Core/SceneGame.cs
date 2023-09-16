using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SceneLib.Interfaces;
using SceneLib.Contexts;
using System;
using SceneLib.Interfaces.Setups;

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
            Project.Kernel.Initialize();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Project.Kernel.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Project.Kernel.Update(gameTime);
            ActiveScene?.Kernel.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            Project.Kernel.Draw(gameTime);
            ActiveScene?.Kernel.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            ActiveScene?.Kernel.Unload(Content);
            Project.Kernel.Unload(Content);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ActiveScene?.Kernel.Dispose();
                Project.Kernel.Dispose();
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
            ActiveScene?.Kernel.Dispose();
            ActiveScene?.Kernel.Unload(Content);
            ActiveScene = Scenes[typeof(TScene)];
            ActiveScene.Kernel.Load(Content);
            ActiveScene.Kernel.Initialize();
        }
    }
}