using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Core;

namespace UILib.Interfaces.Core
{
    public interface IElementChild
    {
        public bool IsActive { get; }
        public bool IsActiveInHierarchy { get; }
        public UIElement Parent { get; }
        public void SetActive(bool active);

        public void SetOrderInParent(int index);

        public abstract void Enable();
        public abstract void Disable();
    }
}
