using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SceneLib.Contexts;
using SceneLib.Interfaces.Kernel.Base;
using SceneLib.Templates.Base.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLib.Templates.Loaders
{
    public class SpriteBatchLoader : BaseValueHandler<SpriteBatch>, IServiceSetup, IContentLoad
    {
        private Game _game;

        public void Setup(ServiceContext context)
        {
            _game = context.Game;
        }
        public void Load(ContentManager manager)
        {
            Value = new SpriteBatch(_game.GraphicsDevice);
        }
    }
}
