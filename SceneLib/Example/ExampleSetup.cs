using SceneLib.Contexts;
using SceneLib.Interfaces.Setups;

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
