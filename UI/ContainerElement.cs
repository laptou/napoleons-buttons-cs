using System;

namespace NapoleonsButtons.UI
{
    public abstract class ContainerElement<T> : Element, IHasChild
        where T : Element
    {
        private T _child;

        protected ContainerElement(T child)
        {
            _child = child;
        }

        public T Child
        {
            get => _child;
            set
            {
                var oldChild = _child;
                _child = value;
                OnChildChanged(_child, oldChild);
            }
        }

        Element IHasChild.Child
        {
            get => Child;
            set
            {
                if (value is T tChild)
                    Child = tChild;
                else
                    throw new InvalidCastException($"{value} is not a {typeof(T)}");
            }
        }

        protected virtual void OnChildChanged(T newChild, T oldChild)
        {
            InvalidateMeasure();
        }
    }
}