using RenderHierarchyLib.Models.Transform;
using Microsoft.Xna.Framework;
using System;

namespace RenderHierarchyLib.Extensions
{
    public static class TransformObjectExtensions
    {
        public static TransformObject SetParentNewTransform(this TransformObject self, TransformObject parent)
        {
            var output = new TransformObject();
            SetParent(ref self.Pos, ref self.Rot, ref self.Sca, 
                ref parent.Pos, ref parent.Rot, ref parent.Sca,
                ref output.Pos, ref output.Rot, ref output.Sca);
            return output;
        }
        public static void SetParentSelfTransform(this ref TransformObject self, TransformObject parent)
        {
            SetParent(ref self.Pos, ref self.Rot, ref self.Sca,
                ref parent.Pos, ref parent.Rot, ref parent.Sca,
                ref self.Pos, ref self.Rot, ref self.Sca);
        }

        public static void SetParent(ref Vector2 selfPos, ref float selfRot, ref Vector2 selfSca,
            ref Vector2 parentPos, ref float parentRot, ref Vector2 parentSca,
            ref Vector2 outputPos, ref float outputRot, ref Vector2 outputSca)
        {
            outputPos = MathExtensions.RotateVector(selfPos * parentSca, parentPos, parentRot);
            outputSca = selfSca * parentSca;
            outputRot = selfRot + parentRot;
        }
    }
}
