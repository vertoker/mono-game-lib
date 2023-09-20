using RenderHierarchyLib.Models.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib
{
    public class Camera
    {
        private readonly TransformCamera _transform;

        public Camera(TransformCamera transform)
        {
            _transform = transform;
        }
    }
}
