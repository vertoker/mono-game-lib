using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Models.Text
{
    public unsafe class UnsafeList<T>
    {
        private const int DefaultCapacity = 16;

        public T[] Items { get; set; }
        public int Size { get; set; }

        private static readonly T[] _emptyArray = Array.Empty<T>();

        public UnsafeList()
        {
            Items = new T[DefaultCapacity];
        }
        public UnsafeList(int startCapacity)
        {
            Items = new T[startCapacity];
        }

        public int Capacity
        {
            get => Items.Length;
            set
            {
                if (value < Size) return;
                if (value == Items.Length) return;

                if (value > 0)
                {
                    var newItems = new T[value];
                    Array.Copy(Items, newItems, Size);
                    Items = newItems;
                }
                else
                {
                    Items = _emptyArray;
                }
            }
        }

        private void Grow(int capacity)
        {
            int newcapacity = Items.Length == 0 ? DefaultCapacity : 2 * Items.Length;
            if (newcapacity < capacity)
                newcapacity = capacity;
            Capacity = newcapacity;
        }

        public void EnsureCapacity(int capacity)
        {
            if (Items.Length < capacity)
                Grow(capacity);
        }

        public void Clear()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                int size = Size;
                Size = 0;
                if (size > 0)
                    Array.Clear(Items, 0, size);
            }
            else
            {
                Size = 0;
            }
        }
    }
}
