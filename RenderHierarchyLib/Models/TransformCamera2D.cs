using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Models
{
    public struct TransformCamera2D
    {
        private Vector2 _pos;
        private float _rot;
        private float _sca;

        public TransformCamera2D(float pixelScale = 2)
        {
            _pos = Vector2.Zero;
            _rot = 0;
            _sca = pixelScale;
        }
    }
}
