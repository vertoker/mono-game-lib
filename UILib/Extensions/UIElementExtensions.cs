using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UILib.Core;

namespace UILib.Extensions
{
    public static class UIElementExtensions
    {
        public static void CheckoutAll(this UIElement parent, Action<UIElement> func)
        {
            var elements = new Stack<UIElement>();
            elements.Push(parent);

            while (elements.TryPop(out var element))
            {
                func.Invoke(element);
                var list = element.Childen;
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    elements.Push(list[i]);
            }
        }

        public static void GetAllInheritors(this UIElement parent)
        {
            var elements = new List<UIElement>();
            parent.GetAllInheritors(ref elements);
        }
        public static void GetAllInheritors(this UIElement parent, ref List<UIElement> elements)
        {
            var list = parent.Childen;
            var count = list.Count;
            if (count == 0) return;

            elements.AddRange(list);
            for (int i = 0; i < count; i++)
                list[i].GetAllInheritors(ref elements);
        }

        public static TElement FindInChildren<TElement>(this UIElement element) where TElement : UIElement
        {
            var list = element.Childen;
            var count = list.Count;
            for (var i = 0; i < count; i++)
                if (list[i] is TElement childElement)
                    return childElement;
            return null;
        }
        public static TElement FindInInheritors<TElement>(this UIElement parent) where TElement : UIElement
        {
            var elements = new Queue<UIElement>();
            elements.Enqueue(parent);

            while (elements.TryDequeue(out var element))
            {
                if (element is TElement childElement)
                    return childElement;
                var list = element.Childen;
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    elements.Enqueue(list[i]);
            }

            return null;
        }

        public static UIElement FindInChildren(this UIElement element, string name)
        {
            var list = element.Childen;
            var count = list.Count;
            for (var i = 0; i < count; i++)
                if (list[i].Name == name)
                    return list[i];
            return null;
        }
        public static UIElement FindInInheritors(this UIElement parent, string name)
        {
            var elements = new Queue<UIElement>();
            elements.Enqueue(parent);

            while (elements.TryDequeue(out var element))
            {
                if (element.Name == name)
                    return element;
                var list = element.Childen;
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    elements.Enqueue(list[i]);
            }

            return null;
        }

        public static TElement FindInChildren<TElement>(this UIElement element, string name) where TElement : UIElement
        {
            var list = element.Childen;
            var count = list.Count;
            for (var i = 0; i < count; i++)
                if (list[i] is TElement childElement && childElement.Name == name)
                    return childElement;
            return null;
        }
        public static TElement FindInInheritors<TElement>(this UIElement parent, string name) where TElement : UIElement
        {
            var elements = new Queue<UIElement>();
            elements.Enqueue(parent);

            while (elements.TryDequeue(out var element))
            {
                if (element is TElement childElement && childElement.Name == name)
                    return childElement;
                var list = element.Childen;
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    elements.Enqueue(list[i]);
            }

            return null;
        }
    }
}
