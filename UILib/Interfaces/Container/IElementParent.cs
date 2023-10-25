using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Core;

namespace UILib.Interfaces.Core
{
    public interface IElementParent
    {
        public IReadOnlyList<UIElement> Childs { get; }

        public void AddChild(UIElement element);
        public void RemoveChild(UIElement element);

        public void SetOrder(UIElement element, int index);

        public bool IsChild();
        public bool IsChild(out IElementChild child);
    }
}
