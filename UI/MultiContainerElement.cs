using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NapoleonsButtons.UI
{
    public abstract class MultiContainerElement<T> : Element, IHasChildren
        where T : Element
    {
        private readonly ObservableCollection<T> _children = new ObservableCollection<T>();

        protected MultiContainerElement(params T[] children)
        {
            _children.CollectionChanged += OnChildrenChanged;
            foreach (var child in children)
                _children.Add(child);
        }

        public IList<T> Children => _children;

        void IHasChildren.AddChild(Element element)
        {
            if (element is T tElement)
                AddChild(tElement);
            else throw new ArgumentException($"Element {element} is not a {typeof(T)}");
        }

        void IHasChildren.RemoveChild(Element element)
        {
            if (element is T tElement)
                RemoveChild(tElement);
            else throw new ArgumentException($"Element {element} is not a {typeof(T)}");
        }

        IEnumerable<Element> IHasChildren.Children => _children;

        protected virtual void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        public void AddChild(T element)
        {
            _children.Add(element);
            element.Parent = this;
        }

        public void RemoveChild(T element)
        {
            _children.Remove(element);
            element.Parent = null;
        }
    }
}