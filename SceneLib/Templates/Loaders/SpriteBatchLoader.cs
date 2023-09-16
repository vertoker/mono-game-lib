using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SceneLib.Interfaces.Kernel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Templates.Loaders
{
    public class SpriteBatchLoader : IContentLoad
    {
        private readonly Game _game;

        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteBatchLoader(Game game)
        {
            _game = game;
        }
        public void Load(ContentManager manager)
        {
            SpriteBatch = new SpriteBatch(_game.GraphicsDevice);
        }
    }
}
