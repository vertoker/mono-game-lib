using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Interfaces.Core;

namespace UILib.Extensions
{
    public static class ParentExtensions
    {
        public static bool IsHierarchyActive(this IElementChild child)
        {
            if (child.IsActiveInHierarchy) return true;
            var parent = child.Parent;
            while (parent != null)
            {
                if (!parent.IsChild(out child)) return false;
                if (child.IsActiveInHierarchy) return true;
                parent = child.Parent;
            }
            return false;
        }
    }
}
