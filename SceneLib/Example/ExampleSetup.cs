using SceneLib.Interfaces;
using SceneLib.Contexts;

namespace SceneLib.Example
{
    public class ExampleSetup : IGameSetup
    {
        public void Setup(GameContext context)
        {
            context.AddProject(new ExampleProjectScene());
        }
    }
}
