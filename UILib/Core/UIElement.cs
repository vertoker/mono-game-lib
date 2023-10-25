using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UILib.Extensions;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public abstract class UIElement : IElementChild, IElementParent, IUIUpdate, IUIDraw
    {
        #region Fields Child
        private bool _selfActive = true;
        private bool _cachedActive = true;
        private bool _enabled = false;
        private IElementParent _parent;

        public bool IsActive => _selfActive;
        public bool IsActiveInHierarchy => _cachedActive;
        public IElementParent Parent => _parent;
        #endregion

        #region Fields Parent
        private readonly List<UIElement> _childrens = new();
        public IReadOnlyList<UIElement> Childs => _childrens;
        #endregion

        #region Methods Child
        public void SetActive(bool active)
        {
            _selfActive = active;
            SetActiveInHierarchy(this.IsHierarchyActive());
        }
        private void SetActiveInHierarchy(bool active)
        {
            if (_cachedActive == active) return;
            _cachedActive = active;
            CheckEnabled();

            foreach (var child in _childrens)
                child.SetActiveInHierarchy(active);
        }

        public void SetOrderInParent(int index)
        {
            if (_parent == null) return;
            _parent.SetOrder(this, index);
        }
        public void SetOrder(UIElement element, int index)
        {
            var length = _childrens.Count - 1;
            if (index < 0 || index > length) return;

            //_childrens.Insert(index, element);
            //_childrens.RemoveAt(length);
        }

        private void CheckEnabled()
        {
            if (_enabled)
            {
                if (!_selfActive || !_cachedActive)
                {
                    _enabled = false;
                    Disable();
                }
            }
            else
            {
                if (_selfActive && _cachedActive)
                {
                    _enabled = true;
                    Enable();
                }
            }
        }
        #endregion

        #region Callbacks
        public virtual void Update(GameTime time)
        {
            if (!_enabled) return;
            foreach (var child in _childrens)
                child.Update(time);
        }
        public virtual void Draw(GameTime time)
        {
            if (!_enabled) return;
            foreach (var child in _childrens)
                child.Draw(time);
        }
        public virtual void Enable() { }
        public virtual void Disable() { }
        #endregion

        #region Methods Parent
        public void AddChild(UIElement element)
        {
            _childrens.Add(element);
            element._parent = this;
        }
        public void RemoveChild(UIElement element)
        {
            _childrens.Remove(element);
            element._parent = null;
        }
        #endregion

        #region Interfaces Convert
        public virtual bool IsParent() => true;
        public virtual bool IsParent(out IElementParent parent)
        {
            parent = this;
            return true;
        }
        public virtual bool IsChild() => true;
        public virtual bool IsChild(out IElementChild child)
        {
            child = this;
            return true;
        }
        #endregion
    }
}
