using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Models.Transform.Interfaces
{
    public interface ITransformLocation
    {
        public Vector2 Pos { get; }
        public float Rot { get; }
    }
}
