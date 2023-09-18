using Microsoft.Xna.Framework;

namespace RenderHierarchyLib.Models
{
    public struct Transform2D
    {
        private Vector2 _pos;
        private float _rot;
        private Vector2 _sca;

        public Transform2D()
        {
            _pos = Vector2.Zero;
            _rot = 0;
            _sca = Vector2.One;
        }
    }
}
