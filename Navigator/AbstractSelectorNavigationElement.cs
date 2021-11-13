using System;
using System.Linq.Expressions;

namespace Navigator
{
    internal abstract class AbstractSelectorNavigationElement<TParent, T>
        where TParent : class
        where T : class
    {
        private readonly INavigationElement<TParent> parent;
        private readonly Expression<Func<TParent, T>> selector;

        private T value;

        public AbstractSelectorNavigationElement(
            INavigationElement<TParent> parent,
            Expression<Func<TParent, T>> selector)
        {
            this.parent = parent;
            this.selector = selector;
        }

        public T GetValue()
        {
            if (!TryGetValue(out var value))
            {
                throw new InvalidNavigationException();
            }

            return value;
        }

        public bool IsValid()
        {
            return TryGetValue(out var _);
        }

        public bool TryGetValue(out T value)
        {
            if (this.value != default)
            {
                value = this.value;
                return true;
            }

            if (!parent.TryGetValue(out var parentValue))
            {
                value = default;
                return false;
            }

            try
            {
                this.value = selector.Compile().Invoke(parentValue);
                value = this.value;
                return true;
            }
            catch (NullReferenceException)
            {
                value = default;
                return false;
            }
        }
    }
}
