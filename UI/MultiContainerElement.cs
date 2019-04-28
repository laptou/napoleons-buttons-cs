using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NapoleonsButtons.UI
{
    public abstract class MultiContainerElement : Element, IHasChildren
    {
        private readonly ObservableCollection<Element> _children = new ObservableCollection<Element>();

        protected MultiContainerElement(params Element[] children)
        {
            foreach (var child in children)
                _children.Add(child);
        }

        public IList<Element> Children => _children;

        IEnumerable<Element> IHasChildren.Children => _children;

        public void AddChild(Element element)
        {
            _children.Add(element);
            element.Parent = this;
        }

        public void RemoveChild(Element element)
        {
            _children.Remove(element);
            element.Parent = null;
        }
    }
}