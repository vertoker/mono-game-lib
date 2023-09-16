using SceneFramework.Core.Model;
using SceneFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneFramework.Example
{
    public class ExampleSetup : IGameSetup
    {
        public void Setup(ISceneSetup setup, GameContext context)
        {
            setup.AddProject(new ProjectScene());
        }
    }
}
