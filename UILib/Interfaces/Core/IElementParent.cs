using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UILib.Interfaces.Core
{
    public interface IElementParent
    {
        public IReadOnlyList<IElementChild> Childs { get; }

        public void AddChild(IElementChild element);
        public void RemoveChild(IElementChild element);

        public bool IsChild();
        public bool IsChild(out IElementChild child);
    }
}
