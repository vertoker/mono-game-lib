using RenderHierarchyLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Core
{
    public class Camera
    {
        private readonly TransformCamera2D _transform;

        public Camera(TransformCamera2D transform)
        {
            _transform = transform;
        }
    }
}
