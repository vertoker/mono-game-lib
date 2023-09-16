using Microsoft.Xna.Framework;
using SceneFramework.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneFramework.Interfaces
{
    public interface IGameSetup
    {
        public void Setup(ISceneSetup setup, GameContext context);
    }
}
