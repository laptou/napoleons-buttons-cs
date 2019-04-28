namespace NapoleonsButtons.UI
{
    public abstract class ContainerElement : Element, IHasChild
    {
        private Element _child;

        protected ContainerElement(Element child)
        {
            _child = child;
        }

        public Element Child
        {
            get => _child;
            set
            {
                var oldChild = _child;
                _child = value;
                OnChildChanged(_child, oldChild);
            }
        }

        protected virtual void OnChildChanged(Element newChild, Element oldChild)
        {
            InvalidateMeasure();
        }
    }
}