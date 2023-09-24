﻿using Microsoft.Xna.Framework;
using RenderHierarchyLib.Models.Enum;
using RenderHierarchyLib.Models.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void SetParent(ref Vector2 selfPos, ref float selfRot, ref Vector2 selfSca, ref Anchor selfAnchor, ref Anchor selfPivot,
            ref Vector2 parentPos, ref float parentRot, ref Vector2 parentSca, ref Anchor parentAnchor, ref Anchor parentPivot,
            ref Vector2 outputPos, ref float outputRot, ref Vector2 outputSca, ref Anchor outputAnchor, ref Anchor outputPivot)
        {
            var offset = selfAnchor.GetCenterRectangle(Vector2.One) - parentPivot.GetCenterRectangle(Vector2.One);
            outputPos = MathExtensions.RotateVector((selfPos + offset) * parentSca, parentPos, parentRot);
            outputSca = selfSca * parentSca;
            outputRot = selfRot + parentRot;
            outputPivot = selfPivot;
            outputAnchor = parentAnchor;
        }
    }
}
