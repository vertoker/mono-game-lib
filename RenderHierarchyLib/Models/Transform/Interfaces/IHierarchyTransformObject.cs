using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Enum;

namespace RenderHierarchyLib.Models.Transform.Interfaces
{
    public interface IHierarchyTransformObject : ITransformObject
    {
        public Anchor Anchor { get; }
        public Anchor Pivot { get; }
        public int Depth { get; }
    }
}
