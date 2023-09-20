using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Models.Transform.Interfaces
{
    public interface ITransformCamera : ITransformLocation
    {
        public float Sca { get; }
    }
}
