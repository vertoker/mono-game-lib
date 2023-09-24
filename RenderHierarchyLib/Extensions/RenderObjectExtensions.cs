using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Transform;

namespace RenderHierarchyLib.Extensions
{
    public static class RenderObjectExtensions
    {
        public static RenderObject SetParentNewTransform(this RenderObject self, RenderObject parent)
        {
            var output = new RenderObject()
            {
                Anchor = self.Anchor,
                Pivot = self.Pivot,
                Depth = self.Depth
            };
            SetParent(ref self.Pos, ref self.Rot, ref self.Sca, ref self.Anchor, ref self.Pivot,
                ref parent.Pos, ref parent.Rot, ref parent.Sca, ref parent.Anchor, ref parent.Pivot,
                ref output.Pos, ref output.Rot, ref output.Sca, ref output.Anchor, ref output.Pivot);
            return output;
        }
        public static void SetParentSelfTransform(this ref RenderObject self, RenderObject parent)
        {
            SetParent(ref self.Pos, ref self.Rot, ref self.Sca, ref self.Anchor, ref self.Pivot,
                ref parent.Pos, ref parent.Rot, ref parent.Sca, ref parent.Anchor, ref parent.Pivot,
                ref self.Pos, ref self.Rot, ref self.Sca, ref self.Anchor, ref self.Pivot);
        }

        public static void SetParent(ref Vector2 selfPos, ref float selfRot, ref Vector2 selfSca, ref Vector2 selfAnchor, ref Vector2 selfPivot,
            ref Vector2 parentPos, ref float parentRot, ref Vector2 parentSca, ref Vector2 parentAnchor, ref Vector2 parentPivot,
            ref Vector2 outputPos, ref float outputRot, ref Vector2 outputSca, ref Vector2 outputAnchor, ref Vector2 outputPivot)
        {
            var offset = (selfAnchor - parentPivot) / 2f;
            outputPos = MathExtensions.RotateVector((selfPos + offset) * parentSca, parentPos, -parentRot);
            outputSca = selfSca * parentSca;
            outputRot = selfRot + parentRot;
            outputPivot = selfPivot;
            outputAnchor = parentAnchor;
        }
    }
}
