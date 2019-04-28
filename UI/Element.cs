using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NapoleonsButtons.UI
{
    public abstract class Element
    {
        #region Hierarchy

        public Element? Parent { get; internal set; }

        #endregion

        #region Rendering

        public void InvalidateRender()
        {
            Parent?.InvalidateRender();
            RenderInvalidated?.Invoke(this, null);
        }

        public abstract Render Render(RenderParameters parameters);

        public event EventHandler RenderInvalidated;

        #endregion

        #region Layout

        public event EventHandler MeasureInvalidated;

        public virtual Size DesiredSize { get; private set; }

        public virtual Size GivenSize { get; private set; }

        public virtual Size Measure(Size availableSize)
        {
            DesiredSize = MeasureOverride(availableSize);
            return DesiredSize;
        }

        protected abstract Size MeasureOverride(Size availableSize);

        public virtual void Arrange(Size actualSize)
        {
            GivenSize = actualSize;
            ArrangeOverride(actualSize);
        }

        protected abstract void ArrangeOverride(Size actualSize);

        public virtual void InvalidateMeasure()
        {
            Parent?.InvalidateMeasure();
            MeasureInvalidated?.Invoke(this, null);
        }

        #endregion

        #region Input

        #endregion

        #region Data

        private object _viewModel;

        public object ViewModel
        {
            get => _viewModel ?? Parent?.ViewModel;
            set
            {
                _viewModel = value;
                ViewModelChanged?.Invoke(this, null);
            }
        }

        public event EventHandler ViewModelChanged;

        #endregion
    }

    public class ValueChangedEventArgs<T> : EventArgs
    {
        public ValueChangedEventArgs(T newValue, T oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }

        public T NewValue { get; }
        public T OldValue { get; }
    }

    public abstract class Value<T>
    {
        protected WeakReference<Element> Element { get; private set; }

        public virtual void Attach(Element element)
        {
            Element = new WeakReference<Element>(element);
        }

        public virtual void Detach()
        {
            Element = null;
        }

        public abstract T Get();

        public event EventHandler<ValueChangedEventArgs<T>> Updated;

        protected void RaiseUpdated(T newValue, T oldValue)
        {
            Updated?.Invoke(this, new ValueChangedEventArgs<T>(newValue, oldValue));
        }
    }

    // TODO: implement binding system
    /*public class StaticValue<T> : Value<T>
    {
        private T _value;

        public StaticValue(T value)
        {
            _value = value;
        }

        public override T Get() => _value;

        public void Set(T value)
        {
            var oldValue = _value;
            _value = value;
            RaiseUpdated(value, oldValue);
        }

        public static implicit operator StaticValue<T>(T value) => new StaticValue<T>(value);
    }

    public class BoundValue<T> : Value<T>
    {
        private class Visitor : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {
                var info = node.Member;

                if (info.MemberType != MemberTypes.Property)
                    return base.VisitMember(node);

                Debug.WriteLine($"found member access: ${node}");

                return base.VisitMember(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return base.VisitParameter(node);
            }
        }

        public delegate T BindingResolver(Element element, object viewModel);

        private BindingResolver _resolver;
        private T               _value;

        public BoundValue(Expression<BindingResolver> expression)
        {
            var visitor = new Visitor();
            visitor.Visit(expression);

            _resolver = expression.Compile();
        }

        public override T Get()
        {
            return _value;
        }

        public override void Attach(Element element)
        {
            element.ViewModelChanged += ElementOnViewModelChanged;
            _value = _resolver(element, element.ViewModel);
            
            base.Attach(element);
        }

        public override void Detach()
        {
            if (Element.TryGetTarget(out var element))
            {
                element.ViewModelChanged -= ElementOnViewModelChanged;
            }
            
            base.Detach();
        }

        private void ElementOnViewModelChanged(object sender, EventArgs e)
        {
            if (Element.TryGetTarget(out var element))
                _value = _resolver(element, element.ViewModel);
        }
    }*/

    public interface IHasChildren
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        IEnumerable<Element> Children { get; }

        void AddChild(Element element);
        void RemoveChild(Element element);
    }

    public interface IHasChild
    {
        Element Child { get; set; }
    }
}