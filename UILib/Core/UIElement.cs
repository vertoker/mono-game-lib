using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public abstract class UIElement : IElementChild, IElementParent
    {
        #region Fields Child
        private bool _selfActive = true;
        private bool _cachedActive = true;
        private bool _enabled = false;

        public bool IsActive => _selfActive;
        public bool IsActiveInHierarchy => _cachedActive;
        public bool IsEnabled => _enabled;
        #endregion

        #region Fields Parent
        private readonly List<IElementChild> _childrens = new();
        public IReadOnlyList<IElementChild> Childs => _childrens;
        #endregion

        #region Methods Child
        public void SetActive(bool active)
        {
            _selfActive = active;
            CheckEnabled();

            foreach (var child in _childrens)
                child.SetActiveInHierarchy(active);
        }
        public void SetActiveInHierarchy(bool active)
        {
            _cachedActive = active;
            CheckEnabled();

            foreach (var child in _childrens)
                child.SetActiveInHierarchy(active);
        }

        private void CheckEnabled()
        {
            if (_enabled)
            {
                if (!_selfActive || !_cachedActive)
                    TryDisable();
            }
            else
            {
                if (_selfActive && _cachedActive)
                    TryEnable();
            }
        }
        public void TryEnable()
        {
            if (_enabled) return;
            _enabled = true;
            Enable();
        }
        public void TryDisable()
        {
            if (!_enabled) return;
            _enabled = false;
            Enable();
        }
        public abstract void Enable();
        public abstract void Disable();
        #endregion

        #region Methods Parent
        public void AddChild(IElementChild element)
        {
            _childrens.Add(element);
        }
        public void RemoveChild(IElementChild element)
        {
            _childrens.Remove(element);
        }
        #endregion

        #region Interfaces Convert
        public bool IsParent() => true;
        public bool IsParent(out IElementParent parent)
        {
            parent = this;
            return true;
        }
        public bool IsChild() => true;
        public bool IsChild(out IElementChild child)
        {
            child = this;
            return true;
        }
        #endregion
    }
}
