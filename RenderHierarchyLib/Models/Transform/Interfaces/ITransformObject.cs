using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Models.Transform.Interfaces
{
    public interface ITransformObject : ITransformLocation
    {
        public Vector2 Sca { get; }
    }
}
