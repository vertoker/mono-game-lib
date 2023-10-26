using Microsoft.Xna.Framework;
using RenderHierarchyLib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UILib.Extensions;
using UILib.Interfaces.Container;
using UILib.Interfaces.Core;

namespace UILib.Core
{
    public abstract class UIElement : IElementChild, IElementRect, IElementParent, IUIUpdate, IUIDraw
    {
        private bool _selfActive = true;
        private bool _cachedActive = true;
        private bool _enabled = false;
        private IElementParent _parent;
        protected UI UI;

        public bool IsActive => _selfActive;
        public bool IsActiveInHierarchy => _cachedActive;
        public IElementParent Parent => _parent;



        private bool _isDirty = false;
        public bool IsDirty => _isDirty;

        private Vector2 _position = Vector2.Zero;
        private float _rotation = 0f;
        private Vector2 _size = Vector2.One;

        public Vector2 Position { get { return _position; } set { _position = value; SetDirty(); } }
        public float Rotation { get { return _rotation; } set { _rotation = value; SetDirty(); } }
        public Vector2 Size { get { return _size; } set { _size = value; SetDirty(); } }

        private Vector2 _pivot = AnchorPresets.CenterMiddle;
        private Vector2 _anchor = PivotPresets.CenterMiddle;

        public Vector2 Pivot { get { return _pivot; } set { _pivot = value; SetDirty(); } }
        public Vector2 Anchor { get { return _anchor; } set { _anchor = value; SetDirty(); } }



        private readonly List<UIElement> _children = new();
        public IReadOnlyList<UIElement> Childen => _children;



        public void SetActive(bool active)
        {
            _selfActive = active;
            SetActiveInHierarchy(this.IsHierarchyActive());
        }
        private void SetActiveInHierarchy(bool active)
        {
            _cachedActive = active;
            CheckEnabled();

            foreach (var child in _children)
                child.SetActiveInHierarchy(active);
        }
        public void SetDirty()
        {
            _isDirty = true;
        }

        public void SetOrderInParent(int index)
        {
            if (_parent == null) return;
            _parent.SetOrder(this, index);
        }
        public void SetOrder(UIElement element, int index)
        {
            var length = _children.Count - 1;
            if (index < 0 || index > length) return;

            _children.Insert(index, element);
            _children.RemoveAt(length);
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



        public void Update(GameTime time)
        {
            if (!_enabled) return;

            if (_isDirty)
            {
                _isDirty = false;
                UpdateRect();
            }

            UpdateElement(time);

            var length = _children.Count;
            if (length == 0) return;
            for (int i = 0; i < length; i++)
                _children[i].Update(time);
        }
        public void Draw(GameTime time)
        {
            if (!_enabled) return;

            if (_isDirty)
            {
                _isDirty = false;
                UpdateRect();
            }

            DrawElement(time);

            var length = _children.Count;
            if (length == 0) return;
            for (int i = 0; i < length; i++)
                _children[i].Draw(time);
        }

        public virtual void UpdateRect() { }
        public virtual void UpdateElement(GameTime time) { }
        public virtual void DrawElement(GameTime time) { }
        public virtual void Enable() { }
        public virtual void Disable() { }



        public void AddChild(UIElement element)
        {
            _children.Add(element);
            element._parent = this;
            element.UI = UI;
            element.SetActive(true);
        }
        public void RemoveChild(UIElement element)
        {
            _children.Remove(element);
            element.SetActive(false);
            element._parent = null;
            element.UI = null;
        }



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
    }
}
