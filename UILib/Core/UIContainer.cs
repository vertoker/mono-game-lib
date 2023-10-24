using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public class UIContainer : IElementParent
    {
        private readonly List<IElementChild> _childrens = new();

        public IReadOnlyList<IElementChild> Childs => _childrens;

        public void AddChild(IElementChild element)
        {
            _childrens.Add(element);
            element.SetActiveInHierarchy(true);
        }
        public void RemoveChild(IElementChild element)
        {
            _childrens.Remove(element);
            element.SetActiveInHierarchy(false);
        }

        public bool IsChild() => false;
        public bool IsChild(out IElementChild child)
        {
            child = null;
            return false;
        }
    }
}
