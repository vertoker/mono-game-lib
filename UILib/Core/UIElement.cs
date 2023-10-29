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
        public string Name { get; set; }
        private bool _selfActive = true;
        private bool _cachedActive = true;
        private bool _enabled = false;
        private UIElement _parent;
        protected UI UI;

        public bool IsActive => _selfActive;
        public bool IsActiveInHierarchy => _cachedActive;
        public UIElement Parent => _parent;

        public UIElement()
        {
            Name = GetType().UnderlyingSystemType.Name;
        }
        public UIElement(string name)
        {
            Name = name;
        }

        private bool _isDirty = true;
        public bool IsDirty => _isDirty;

        private Vector2 _localPosition = Vector2.Zero;
        private float _localRotation = 0f;
        private Vector2 _localSize = Vector2.One;

        private Vector2 _localPivot = AnchorPresets.CenterMiddle;
        private Vector2 _localAnchor = PivotPresets.CenterMiddle;

        public Vector2 LocalPosition { get { return _localPosition; } set { _localPosition = value; SetDirty(); } }
        public float LocalRotation { get { return _localRotation; } set { _localRotation = value; SetDirty(); } }
        public Vector2 LocalSize { get { return _localSize; } set { _localSize = value; SetDirty(); } }
        public Vector2 LocalPivot { get { return _localPivot; } set { _localPivot = value; SetDirty(); } }
        public Vector2 LocalAnchor { get { return _localAnchor; } set { _localAnchor = value; SetDirty(); } }


        private Vector2 _position = Vector2.Zero;
        private float _rotation = 0f;
        private Vector2 _size = Vector2.One;

        private Vector2 _pivot = AnchorPresets.CenterMiddle;
        private Vector2 _anchor = PivotPresets.CenterMiddle;

        public Vector2 Position => _position;
        public float Rotation => _rotation;
        public Vector2 Size => _size;
        public Vector2 Pivot => _pivot;
        public Vector2 Anchor => _anchor;



        private readonly List<UIElement> _children = new();
        public IReadOnlyList<UIElement> Childen => _children;

        public void SetActive(bool active)
        {
            _selfActive = active;
            SetActiveInHierarchy(IsHierarchyActive(this));
        }
        public static bool IsHierarchyActive(UIElement element)
        {
            while (element != null)
            {
                if (element.IsActiveInHierarchy) return true;
                element = element.Parent;
            }
            return false;
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

            var length = _children.Count;
            if (length == 0) return;
            for (int i = 0; i < length; i++)
                _children[i].SetDirty();
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

            UpdateRect();

            UpdateElement(time);

            var length = _children.Count;
            if (length == 0) return;
            for (int i = 0; i < length; i++)
                _children[i].Update(time);
        }
        public void Draw(GameTime time)
        {
            if (!_enabled) return;

            DrawElement(time);

            var length = _children.Count;
            if (length == 0) return;
            for (int i = 0; i < length; i++)
                _children[i].Draw(time);
        }

        public void UpdateRect()
        {
            if (!_isDirty) return;
            _isDirty = false;

            if (Parent != null)
            {
                UpdateRect(ref Parent._position, ref Parent._localRotation, ref Parent._localSize, ref Parent._localAnchor, ref Parent._localPivot);
            }
            else
            {
                _position = _localPosition;
                _rotation = _localRotation;
                _size = _localSize;
                _anchor = _localAnchor;
                _pivot = _localPivot;

                UpdateChildrenRect();
            }
        }
        private void UpdateRect(ref Vector2 parentPos, ref float parentRot, ref Vector2 parentSca, ref Vector2 parentAnchor, ref Vector2 parentPivot)
        {
            RenderObjectExtensions.SetParent(
                ref _localPosition, ref _localRotation, ref _localSize, ref _localAnchor, ref _localPivot,
                ref parentPos, ref parentRot, ref parentSca, ref parentAnchor, ref parentPivot,
                ref _position, ref _rotation, ref _size, ref _anchor, ref _pivot);

            UpdateChildrenRect();
        }

        private void UpdateChildrenRect()
        {
            var length = _children.Count;
            if (length == 0) return;
            for (int i = 0; i < length; i++)
                _children[i].UpdateRect(ref _position, ref _rotation, ref _size, ref _anchor, ref _pivot);
        }

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
    }
}
