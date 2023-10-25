using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UILib.Interfaces.Core
{
    public interface IElementChild
    {
        public bool IsActive { get; }
        public bool IsActiveInHierarchy { get; }
        public IElementParent Parent { get; }
        public void SetActive(bool active);

        public void SetOrderInParent(int index);

        public abstract void Enable();
        public abstract void Disable();

        public bool IsParent();
        public bool IsParent(out IElementParent parent);
    }
}
